import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatDialog, MatTableDataSource } from '@angular/material';
import { E_VoucherBudgetDetail, E_VoucherBudget, Org_Organization, E_VoucherType, E_VoucherBudgetDenominations } from 'src/app/models/data/data';
import { AppConfig } from 'src/app/services/config/app.config';
import { EvoucherBuggetDistributeService } from 'src/app/services/evoucher-budget/evoucher-budget-dist.service';
import { ActivatedRoute } from '@angular/router';
import { EvoucherBudgetDistLineDialogComponent } from '../evoucher-budget-dist-detail-line/evoucher-budget-dist-detail-line.component';
import { debug } from 'util';
import { asEnumerable } from 'linq-es2015';

@Component({
  selector: 'app-evoucher-budget-dist-detail',
  templateUrl: './evoucher-budget-dist-detail.component.html',
  styleUrls: ['./evoucher-budget-dist-detail.component.css']
})
export class EvoucherBudgetDistDetailComponent implements OnInit {
  filterDate = new Date();
  filterCompanyId: number;
  id: number;
  isProcessing = false;

  // duyet phan bo
  isApproveUser = true;

  // menh gia tien
  denominations: E_VoucherType[];

  // paging
  countTotal = 0;
  page = 0;
  pageSizeOptions = [200, 500, 1000];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns = ['STT', 'codeUser', 'nameUser', 'budget', 'status', 'actions'];
  dataSource: MatTableDataSource<E_VoucherBudgetDetail>;

  item: E_VoucherBudget;
  companies: Org_Organization[];
  status = AppConfig.settings.evoucherBudgetStatus;

  constructor(private _service: EvoucherBuggetDistributeService, private dialog: MatDialog, protected _route: ActivatedRoute) { }

  async ngOnInit() {
    this.id = this._route.snapshot.params['id'];
    this.paginator.pageSize = 5;
    await this.find();
    await this.getDenominations();
  }

  async getDenominations() {
    const res = await this._service.getDenominations();
    if (res.isSuccess) {
      this.denominations = res.data;
    }
  }

  async find() {
    this.resetPaging();

    const res = await this._service.getCompanyBudgetDist({
      evoucherBudgetId: this.id
      , start: 0, length: 0
    });
    if (res.isSuccess) {
      this.item = res.data;
      if (this.item) {
        await this.getDetails();
        this.onPaginateChange({ pageIndex: 0 });
      } else {
        // reset danh sach nhan vien
        this.dataSource = new MatTableDataSource<E_VoucherBudgetDetail>();
      }
    }
  }
  async getDetails() {
    const res = await this._service.getDetails({ evoucherBudgetId: this.item.id, start: 0, length: 0 });
    if (res.isSuccess) {
      this.item.budgetDetails = res.data.pageData;
      this.countTotal = res.data.totalRowCount;
    }
  }

  onPaginateChange(event) {
    this.page = event.pageIndex;
    if (this.item) {
      this.getPaging();
    }
  }

  resetPaging() {
    this.paginator.pageIndex = 0;
  }

  getPaging() {
    const res = this.paginate(this.item.budgetDetails, this.paginator.pageSize, this.page + 1);
    this.dataSource = new MatTableDataSource<E_VoucherBudgetDetail>(res);
    this.countTotal = this.item.budgetDetails.length;
  }

  paginate(array, page_size, page_number) {
    // human-readable page numbers usually start with 1, so we reduce 1 in the first argument
    return array.slice((page_number - 1) * page_size, page_number * page_size);
  }

  // luu du lieu
  async save() {
    const res = await this._service.saveCompanyBudgetDist(this.item);
    if (res.isSuccess) {
      alert('Lưu thông tin phân bổ thành công!');
      await this.find();
    }
  }

  // hoan thanh nhap ngan sach
  async finishDistribute() {
    if (!this.item) {
      return;
    }

    // const res = await this._service.finishBudget(this.item);
    // if (res.isSuccess) {
    //   alert('Hoàn thành nhập ngân sách thành công!');
    //   await this.find();
    // } else {
    //   alert(res.message);
    // }

  }

  // phan bo
  distribute() {
    if (this.item) {
      if (this.item.budgetDetails && this.item.budgetDetails.length > 0) {
        this.item.denomiTotalCount = 0;
        this.item.denomiTotalValues = 0;
        // thong ke menh gia
        this.item.budgetDenominations = [];
        const tmpDenomis: E_VoucherBudgetDenominations[] = [];
        this.denominations.forEach(denomi => {
          tmpDenomis.push({
            countNumber: 0,
            denominations: denomi.denominations,
            totalValues: 0,
            voucherBudgetId: this.item.id,
            voucherTypeCode: denomi.code,
            voucherTypeName: denomi.name
          });
        });

        // duyet qua danh sach nhan vien
        this.item.budgetDetails.forEach(budDetail => {
          budDetail.distributeLines = [];

          // so tien con lai
          let remainMoney = budDetail.budget;

          this.denominations.forEach(denomi => {
            const x = Math.floor(remainMoney / denomi.denominations);
            // chia cho menh gia >= 1
            if (x >= 1) {
              // so tien con lai
              const totalVal = x * denomi.denominations;
              remainMoney = remainMoney - totalVal;

              // them menh gia
              budDetail.distributeLines.push({
                voucherTypeCode: denomi.code,
                denominations: denomi.denominations,
                countNumberPage: x,
                totalValues: totalVal,
                voucherBudgetDetailId: budDetail.id,
                voucherTypeName: denomi.name
              });

              // thong ke menh gia
              const deno = asEnumerable(tmpDenomis).FirstOrDefault(y => y.voucherTypeCode === denomi.code);
              if (deno) {
                deno.countNumber += x;
                deno.totalValues += totalVal;
                this.item.denomiTotalCount += x;
                this.item.denomiTotalValues += totalVal;
              }
            }
          });

          const stat = asEnumerable(AppConfig.settings.evoucherBudgetStatus).FirstOrDefault(x => x.code === 'Distributed');
          if (stat !== null) {
            budDetail.status = stat.id;
            budDetail.statusName = stat.title;
          }

        });

        this.item.budgetDenominations = tmpDenomis;
      }
    }
  }

  showEditLineDialog(line: E_VoucherBudgetDetail) {
    if (!this.item) {
      return;
    }
    // neu da hoan thanh phan bo
    if (this.item.status === 3) {
      return;
    }
    const dialogRef = this.dialog.open(EvoucherBudgetDistLineDialogComponent, {
      width: '800px',
      data: { budgetDate: this.item.budgetDate, item: line }
    });

    dialogRef.afterClosed().subscribe(
      async res => {
        // click OK
        if (res) {
          this.isProcessing = true;

          // da phan bo hoac hoan thanh, phe duyet
          if (this.item.status >= 2) {
            // luu thay doi
            await this.SaveEmployeeBudgetDistributes(res);
            // load lai du lieu tu DB
            await this.find();
          } else {
            // luu thay doi tam thoi
            line = res;
            // load lai thong ke tren giao dien
            this.refreshTotal();
          }
          this.isProcessing = false;
        }
      }
      , err => {
        alert(err);

        console.log(err);
      });
  }

  async SaveEmployeeBudgetDistributes(budgetdetail) {
    const res = await this._service.saveEmployeeBudgetDist(budgetdetail);
    if (res.isSuccess) {
      alert('Lưu thông tin phân bổ nhân viên thành công!');
      await this.find();
    }
  }

  // refresh lai danh sach thong ke khi sua tam tren giao dien
  refreshTotal() {
    if (this.item) {
      if (this.item.budgetDetails && this.item.budgetDetails.length > 0) {
        // thong ke menh gia
        this.item.budgetDenominations = [];
        const tmpDenomis: E_VoucherBudgetDenominations[] = [];
        this.denominations.forEach(denomi => {
          tmpDenomis.push({
            countNumber: 0,
            denominations: denomi.denominations,
            totalValues: 0,
            voucherBudgetId: this.item.id,
            voucherTypeCode: denomi.code,
            voucherTypeName: denomi.name
          });
        });

        // tong so voucher da phan bo
        this.item.denomiTotalCount = 0;
        this.item.denomiTotalValues = 0;

        this.item.budgetDetails.forEach(budDetail => {
          budDetail.distributeLines.forEach(distLine => {
            // thong ke menh gia
            const deno = asEnumerable(tmpDenomis).FirstOrDefault(y => y.voucherTypeCode === distLine.voucherTypeCode);
            if (deno) {
              deno.countNumber += distLine.countNumberPage;
              deno.totalValues += distLine.totalValues;
              this.item.denomiTotalCount += distLine.countNumberPage;
              this.item.denomiTotalValues += distLine.totalValues;
            }
          });
        });

        this.item.budgetDenominations = tmpDenomis;
      }
    }
  }

  async completeDistribute() {
    const res = await this._service.completeDistribute(this.item);
    if (res.isSuccess) {
      alert('Hoàn thành phân bổ thành công!');
      await this.find();
    } else {
      alert(res.message);
    }
  }

  async approve() {
    const res = await this._service.approveDistribute(this.item);
    if (res.isSuccess) {
      alert('Duyệt phân bổ thành công!');
      await this.find();
    } else {
      alert(res.message);
    }
  }

  async return() {
    const res = await this._service.returnDistribute(this.item);
    if (res.isSuccess) {
      alert('Trả lại phân bổ thành công!');
      await this.find();
    } else {
      alert(res.message);
    }
  }
}
