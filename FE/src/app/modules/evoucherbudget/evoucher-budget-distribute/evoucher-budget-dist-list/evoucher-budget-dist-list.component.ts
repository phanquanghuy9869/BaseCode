import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatDialog } from '@angular/material';
import { E_VoucherBudget, Org_Organization, ProcessStatus } from 'src/app/models/data/data';
import { AppConfig } from 'src/app/services/config/app.config';
import { EvoucherBuggetDistributeService } from 'src/app/services/evoucher-budget/evoucher-budget-dist.service';
import { EvoucherBuggetService } from 'src/app/services/evoucher-budget/evoucher-budget.service';

@Component({
  selector: 'app-evoucher-budget-dist-list',
  templateUrl: './evoucher-budget-dist-list.component.html',
  styleUrls: ['./evoucher-budget-dist-list.component.css']
})
export class EvoucherBudgetDistListComponent implements OnInit {
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

  // lay cac trang thai da nhap hoac da phan bo
  status: ProcessStatus[];

  constructor(private _service: EvoucherBuggetDistributeService, private dialog: MatDialog
    , private _budgetService: EvoucherBuggetService, ) { }

  async ngOnInit() {

    this.status = AppConfig.settings.evoucherBudgetStatus.filter(x => x.id >= 1);
    this.status.unshift({ title: 'Tất cả', id: null });
    await this.getCompanies();
  }

  async getCompanies() { 
    const res = await this._budgetService.getCompanies();
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

    const res = await this._service.findBudgetDistributes({
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

}

