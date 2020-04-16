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

@Component({
  selector: 'app-evoucher-budget-list',
  templateUrl: './evoucher-budget-list.component.html',
  styleUrls: ['./evoucher-budget-list.component.css']
})
export class EvoucherBudgetListComponent implements OnInit {
  filterDateFrom = new Date();
  filterDateTo = new Date();
  filterCompanyId: number;
  filterStatus: number;

  isProcessing = false;

  // paging
  start = 1;
  length = 200;
  countTotal = 0;
  page = 0;
  pageSizeOptions = [200, 500, 1000];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['STT', 'budgetDate', 'totalStaff', 'budget', 'statusName', 'actions'];
  dataSource: MatTableDataSource<E_VoucherBudget>;

  companies: Org_Organization[];
  status = AppConfig.settings.evoucherBudgetStatus;

  constructor(private _service: EvoucherBuggetService, private dialog: MatDialog) { }

  async ngOnInit() {
    await this.getCompanies();
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
    await this.getPaging();
  }

  async onPaginateChange(event) {
    this.page = event.pageIndex;
    this.start = this.page * this.length + 1;
    await this.getPaging();
  }

  resetPaging() {
    this.start = 1;
    this.length = this.paginator.pageSize;
    this.paginator.pageIndex = 0;
  }

  async getPaging() {
    this.dataSource = new MatTableDataSource<E_VoucherBudget>();

    const res = await this._service.find({
      dateFrom: this.filterDateFrom,
      dateTo: this.filterDateTo,
      companyId: this.filterCompanyId,
      start: this.start,
      length: this.length,
      status: this.filterStatus
    });

    if (res.isSuccess) {
      this.dataSource = new MatTableDataSource<E_VoucherBudget>(res.data.pageData);
      this.countTotal = res.data.totalRowCount;
    }
  }

  showImportDialog() {
    const dialogRef = this.dialog.open(EvoucherBudgetInputDialogComponent, {
      width: '500px',
      data: { companies: this.companies }
    });

    dialogRef.afterClosed().subscribe(
      async res => {
        // click OK
        if (res) {
          this.isProcessing = true;
          await this.CreateBudget(res);
          this.isProcessing = false;
        }
      }
      , err => {
        alert(err);

        console.log(err);
      });
  }

  async CreateBudget(data: any) {
    const res = await this._service.addVoucherBudget(data);
    if (res.isSuccess) {
      const importRes = res.data;
      if (importRes.isSuccess) {
        // nhap du lieu thanh cong
        const dialogRef = this.dialog.open(EvoucherBudgetInputSuccessDialogComponent, {
          width: '500px',
          data: {
            date: importRes.budgetDate,
            company: importRes.companyName,
            totalStaff: importRes.totalStaff,
            totalBudget: importRes.budget,
          }
        });
      } else {
        // loi xay ra
        // loi khi da hoan thanh nhap ngay nay
        if (importRes.message) {
          alert(importRes.message);
        } else {
          // loi sai ma nhan vien
          const dialogRef = this.dialog.open(EvoucherBudgetInputErrorDialogComponent, {
            width: '800px',
            data: {
              date: importRes.budgetDate,
              company: importRes.companyName,
              totalStaff: importRes.totalStaff,
              totalBudget: importRes.budget,
              errors: importRes.errorLines,
            }
          });
        }
      }
    }
  }
}
