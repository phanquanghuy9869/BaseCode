import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NotificationService } from 'src/app/services/notification/notification.service';
import { KpiNotification } from 'src/app/models/data/data';
import { KpiNotificationFilterModel } from 'src/app/models/base/utilities';
import { BaseGridComponent, BaseGridDatasource } from 'src/app/models/base/base-grid-component';
import { appGlobals } from '../../app-global';

@Component({
  selector: 'app-notification-dialog',
  templateUrl: './notification-dialog.component.html',
  styleUrls: ['./notification-dialog.component.css']
})
export class NotificationDialogComponent extends BaseGridComponent<KpiNotification,
KpiNotificationFilterModel, NotificationService, KpiNotificationDataSource>  {

  displayedColumns = ['userDate', 'notes', 'fromUserName', 'action'];
  start = 0;
  length = 20;
  countTotal: number;

  constructor(public dialogRef: MatDialogRef<NotificationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: {}
    , _service: NotificationService, private _authService: AuthService) {
    super(_service, KpiNotificationDataSource);
  }

  async ngOnInit() {
    await super.ngOnInit();
  }

  getFilter(): KpiNotificationFilterModel {
    if (this.filter == null) {
      this.filter = { start: this.start, length: this.length };
    }

    if (this.paginator.pageIndex == null) {
      this.paginator.pageIndex = this.start;
    }

    if (this.paginator.pageSize == null) {
      this.paginator.pageSize = this.length;
    }

    this.start = this.paginator.pageIndex * this.paginator.pageSize + 1;
    this.length = this.paginator.pageSize;
    this.filter.start = this.start;
    this.filter.length = this.length;
    this.filter.userName = this._authService.getUsername();
    return this.filter;
  }

  async markRead(id: number) {
    const res = await this._service.markRead(id);
    if (res.isSuccess) {
      await this.searchPaging();
    } else {
      console.log(res.message);
    }
  }

  async markUnRead(id: number) {
    const res = await this._service.markUnRead(id);
    if (res.isSuccess) {
      await this.searchPaging();
    } else {
      console.log(res.message);
    }
  }
}

export class KpiNotificationDataSource extends BaseGridDatasource<KpiNotification, KpiNotificationFilterModel, NotificationService> {
}