import { Component, OnInit, ViewChild } from '@angular/core';
import { Org_Organization, E_VoucherBudget, E_VoucherBudgetDetail } from 'src/app/models/data/data';
import { AppConfig } from 'src/app/services/config/app.config';
import { EvoucherBuggetService } from 'src/app/services/evoucher-budget/evoucher-budget.service';
import { MatPaginator, MatTableDataSource, MatDialog } from '@angular/material';
import { tap } from 'rxjs/operators';
import { EvoucherBudgetInputDialogComponent } from '../evoucher-budget-input-dialog/evoucher-budget-input.component';
import { debug } from 'util';
import { EvoucherBudgetInputSuccessDialogComponent } from '../evoucher-budget-input-success-dialog/evoucher-budget-input-success-dialog.component';
import { EvoucherBudgetInputErrorDialogComponent } from '../evoucher-budget-input-error-dialog/evoucher-budget-input-error-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-evoucher-budget-info',
  templateUrl: './evoucher-budget-info.component.html',
  styleUrls: ['./evoucher-budget-info.component.css']
})
export class EvoucherBudgetInfoComponent implements OnInit {
  filterDate = new Date();
  filterCompanyId: number;
  id: number;
  isProcessing = false;

  // paging
  start = 1;
  length = 200;
  countTotal = 0;
  page = 0;
  pageSizeOptions = [200, 500, 1000];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['STT', 'codeUser', 'nameUser', 'budget', 'status', 'createdDate'];
  dataSource: MatTableDataSource<E_VoucherBudgetDetail>;

  item: E_VoucherBudget;
  companies: Org_Organization[];
  status = AppConfig.settings.evoucherBudgetStatus;

  constructor(private _service: EvoucherBuggetService, private dialog: MatDialog, protected _route: ActivatedRoute) { }


  async ngOnInit() {
    this.id = this._route.snapshot.params['id'];
    await this.getCompanies();
    await this.find();
  }

  async getCompanies() {
    const res = await this._service.getCompanies();
    if (res) {
      this.companies = [];
      if (res.isSuccess) {
        this.companies = res.data;
      }
    }
  }

  async find() {
    this.resetPaging();

    const res = await this._service.findById({
      evoucherBudgetId: this.id
      , start: 0, length: 0
    });
    if (res.isSuccess) {
      this.item = res.data;
      if (this.item) {
        await this.onPaginateChange({ pageIndex: 0 });
      } else {
        // reset danh sach nhan vien
        this.dataSource = new MatTableDataSource<E_VoucherBudgetDetail>();
      }
    }
  }

  async onPaginateChange(event) {
    this.page = event.pageIndex;
    this.start = this.page * this.length + 1;
    if (this.item) {
      await this.getPaging();
    }
  }

  resetPaging() {
    this.start = 1;
    this.length = this.paginator.pageSize;
    this.paginator.pageIndex = 0;
  }

  async getPaging() {
    this.dataSource = new MatTableDataSource<E_VoucherBudgetDetail>();

    const res = await this._service.getDetailsPaging({
      start: this.start
      , length: this.length
      , evoucherBudgetId: this.item.id
    });

    if (res.isSuccess) {
      this.dataSource = new MatTableDataSource<E_VoucherBudgetDetail>(res.data.pageData);
      this.countTotal = res.data.totalRowCount;
    }
  }

  // hoan thanh nhap ngan sach
  async finishImport() {
    if (!this.item) {
      return;
    }

    const res = await this._service.finishBudget(this.item);
    if (res.isSuccess) {
      alert('Hoàn thành nhập ngân sách thành công!');
      await this.find();
    } else {
      alert(res.message);
    }

  }
}
