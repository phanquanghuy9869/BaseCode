import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { BaseGridComponent, BaseGridDatasource } from '../../../../../models/base/base-grid-component';
import { EventDiarySyncModel, User } from '../../../../../models/data/data';
import { EventDiarySyncFilterModel } from '../../../../../models/base/utilities';
import { EventService } from '../../../../../services/event-diary/event.service';
import { MonthPickerComponent } from '../../../../share/components/month-picker/month-picker.component';
import { asEnumerable } from 'linq-es2015';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import { appGlobals } from 'src/app/modules/share/app-global';

@Component({
  selector: 'app-missing-config',
  templateUrl: './missing-config.component.html',
  styleUrls: ['./missing-config.component.css']
})
export class MissingConfigComponent extends BaseGridComponent<EventDiarySyncModel, EventDiarySyncFilterModel, EventService, MissingEventDiarySyncDataSource> {
  countTotal: number;
  @Input() yearMonth: number;
  @Input() orgs = [];
  @Input() level1Mngs: User[] = [];
  
  @ViewChild('monthFilter') monthFilter: MonthPickerComponent;
  displayedColumns = ['isSelected', 'STT', 'kpi-month', 'org-name', 'level1ManagerFullName', 'level2ManagerFullName'];
  @Output() changedTrigger = new EventEmitter<number>();
  start = 1;
  length = 5;

  constructor(_service: EventService, private _dialog: CommonDialogService) {
    super(_service, MissingEventDiarySyncDataSource);
  }

  getFilter(): EventDiarySyncFilterModel {
    if (this.filter == null) {
      this.filter = { start: this.start, length: this.length };
    }

    this.filter.yearMonth = this.yearMonth;
    console.log('Add new: ', this.yearMonth);

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
    console.log(this.filter);
    return this.filter;
  }

  searchPaging() {
    // const filter = this.getFilter();
    // this.resetPaging();
    this.getPaging();
  }

  async getPaging() {
    const filter = this.getFilter();
    await this.dataSource.getPaging(this.filter);
    this.count(this.filter);
  }

  checkAll(isChecked) {
    for (let i = 0; i < this.dataSource.data.value.length; i++) {
      const element = this.dataSource.data.value[i];
      element.uiIsSelected = isChecked;
    }
  }

  ngOnInit() {
    super.ngOnInit();
  }

  async diaryAddRange() {
    const model = asEnumerable(this.dataSource.data.value).Where(x => x.uiIsSelected).ToArray();
    if (model == null || model.length == 0) {
      if (appGlobals.getLang()=='vn'){
		this._dialog.alert('Chưa có bản ghi nào được chọn!');
	  }else{
		this._dialog.alert('No records have been selected!');
	  }
      return;
    }

    for (let i = 0; i < model.length; i++) {
      const element = model[i];
      element.yearMonth = this.yearMonth;
    }
    try {
      const rs = await this._service.addRangeDiaryKpi(model);
      if (rs.isSuccess) {
        if (appGlobals.getLang()=='vn'){
          alert('Thay đổi thành công!');
        }else{
          alert('Successful Changed!');
        }
        console.log('Add range complee');
        this.changedTrigger.emit(this.yearMonth);
      } else {
        this._dialog.alert(rs.message);
      }
    } catch (error) {
      this._dialog.alert('Có lỗi xảy ra!');
    }
  }

  async count(filter: EventDiarySyncFilterModel) {
    this.countTotal = this.dataSource.count;
  }
}

export class MissingEventDiarySyncDataSource extends BaseGridDatasource<EventDiarySyncModel, EventDiarySyncFilterModel, EventService> {
  public count: number;

  async getPaging(filter: EventDiarySyncFilterModel) {
    try {
      const response = await this.dataService.getPagingMissingEventDiaryByYearMonth(filter);
      if (response.isSuccess) {
        const data = this.processData(response.data.paging, filter);
        this.data.next(data);
        console.log(response.data);
        this.count = response.data.count;
        return;
      }
    } catch (error) {
      console.log(error.message);
    }
  }
}
