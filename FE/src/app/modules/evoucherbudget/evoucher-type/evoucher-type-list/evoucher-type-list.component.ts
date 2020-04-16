import { Component, OnInit } from '@angular/core';
import { Org_Organization, E_VoucherType } from 'src/app/models/data/data';
import { BaseGridComponent, BaseGridDatasource } from 'src/app/models/base/base-grid-component';
import { OrgFilterModel, EvoucherTypeFilterModel } from 'src/app/models/base/utilities';
import { OrgService } from 'src/app/services/orgs/org.service';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import { ActivatedRoute } from '@angular/router';
import { EvoucherTypeService } from 'src/app/services/evoucher-budget/evoucher-type.service';


@Component({
  selector: 'app-evoucher-type-list',
  templateUrl: './evoucher-type-list.component.html',
  styleUrls: ['./evoucher-type-list.component.css']
})
export class EvoucherTypeListComponent extends BaseGridComponent<E_VoucherType, EvoucherTypeFilterModel
, EvoucherTypeService, EvoucherTypeDataSource>{
  start = 1;
  length = 20;
  countTotal = 0;
  filterName = '';
  filterCode = '';
  filterIsValidate = 'T';

  displayedColumns = ['STT', 'code', 'name', 'denominations', 'oderNumber', 'isValidate', 'actions'];

  constructor(_service: EvoucherTypeService, private _dialogService: CommonDialogService, _route: ActivatedRoute) {
    super(_service, EvoucherTypeDataSource, _route);
  }

  getFilter(): EvoucherTypeFilterModel {
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
    this.filter.name = this.filterName;
    this.filter.code = this.filterCode;
    this.filter.isValidate = this.filterIsValidate;
    return this.filter;
  }

  ngOnInit() {
    super.ngOnInit();
  }

}

export class EvoucherTypeDataSource extends BaseGridDatasource<E_VoucherType, EvoucherTypeFilterModel, EvoucherTypeService> {
}
