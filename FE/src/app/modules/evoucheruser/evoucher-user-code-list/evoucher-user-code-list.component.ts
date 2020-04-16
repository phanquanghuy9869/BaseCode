import { Component, OnInit } from '@angular/core';
import { BaseGridComponent, BaseGridDatasource } from 'src/app/models/base/base-grid-component';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import { ActivatedRoute } from '@angular/router';
import { View_EVoucherUser } from 'src/app/models/data/evoucher';
import {  EvoucherUserCodeFilterModel } from 'src/app/models/base/evoucher';
import { EvoucherUserCodeService } from 'src/app/services/evoucher-user/evoucher-user-code.service';


@Component({
  selector: 'app-evoucher-user-code-list',
  templateUrl: './evoucher-user-code-list.component.html',
  styleUrls: ['./evoucher-user-code-list.component.css']
})
export class EvoucherUserCodeListComponent extends BaseGridComponent<View_EVoucherUser, EvoucherUserCodeFilterModel
, EvoucherUserCodeService, EvoucherCodeLineDataSource>{
  start = 1;
  length = 2000;
  countTotal = 0;
  filterName = '';
  filterCode = '';
  filterIsValidate = 'T';

  displayedColumns = ['STT', 'VoucherTypeCode', 'VoucheTypeName', 'Denominations', 'StartDate', 'EndDate', 'actions'];

  constructor(_service: EvoucherUserCodeService, private _dialogService: CommonDialogService, _route: ActivatedRoute) {
    super(_service, EvoucherCodeLineDataSource, _route);
  }

  getFilter(): EvoucherUserCodeFilterModel {
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
    this.filter.Status = 1;
    //this.filter.code = this.filterCode;
    //this.filter.isValidate = this.filterIsValidate;
    return this.filter;
  }

  ngOnInit() {
    super.ngOnInit();
  }

}

export class EvoucherCodeLineDataSource extends BaseGridDatasource<View_EVoucherUser, EvoucherUserCodeFilterModel, EvoucherUserCodeService> {
}
