import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatTableDataSource } from '@angular/material';
import { E_VoucherBudgetDetail, E_VoucherBudgetDetailLine, E_VoucherType } from 'src/app/models/data/data';
import { EvoucherTypeService } from 'src/app/services/evoucher-budget/evoucher-type.service';
import { EvoucherBuggetService } from 'src/app/services/evoucher-budget/evoucher-budget.service';
import { EvoucherBuggetDistributeService } from 'src/app/services/evoucher-budget/evoucher-budget-dist.service';
import { asEnumerable } from 'linq-es2015';

@Component({
  selector: 'app-evoucher-budget-dist-detail-line',
  templateUrl: './evoucher-budget-dist-detail-line.component.html',
  styleUrls: ['./evoucher-budget-dist-detail-line.component.css']
})
export class EvoucherBudgetDistLineDialogComponent implements OnInit {
  budgetDate: Date;

  item: E_VoucherBudgetDetail;
  voucherTypes: E_VoucherType[];

  // new voucher
  newVoucherId: number;
  newVoucherQty: number;

  displayedColumns = ['STT', 'voucherTypeName', 'denominations', 'countNumberPage', 'totalValues', 'actions'];
  dataSource: MatTableDataSource<E_VoucherBudgetDetailLine>;

  constructor(public dialogRef: MatDialogRef<EvoucherBudgetDistLineDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public data: any, private _vcTypeService: EvoucherBuggetDistributeService) {
    this.item = data.item;
    this.budgetDate = data.budgetDate;
  }

  async  ngOnInit() {
    const res = await this._vcTypeService.getDenominations();
    if (res.isSuccess) {
      this.voucherTypes = res.data.filter(x => x.isValidate);
    }

    this.dataSource = new MatTableDataSource<E_VoucherBudgetDetailLine>(this.item.distributeLines);
  }

  addVoucher() {
    if (this.newVoucherQty) {
      this.newVoucherQty = Math.abs(Math.floor(this.newVoucherQty));
      if (this.newVoucherQty === 0) {
        alert('Số lượng phải lớn hơn 0');
      }
    } else {
      alert('Chưa nhập số lượng');
      return;
    }

    // lay voucher trong combobox
    const voucher = asEnumerable(this.voucherTypes).FirstOrDefault(x => x.id === this.newVoucherId);
    if (voucher) {
      // kiem tra tong tien so voi ngan sach
      let curTotalValues = asEnumerable(this.item.distributeLines).Sum(x => x.totalValues);
      curTotalValues += this.newVoucherQty * voucher.denominations;
      if (curTotalValues > this.item.budget) {
        alert('Tổng số tiền vượt quá ngân sách');
        return;
      }

      const line = asEnumerable(this.item.distributeLines).FirstOrDefault(x => x.voucherTypeCode === voucher.code);
      // kiem tra voucher da có trong danh sach chua
      if (!line) {
        const newItem = {
          voucherTypeCode: voucher.code,
          voucherTypeName: voucher.name,
          denominations: voucher.denominations,
          countNumberPage: this.newVoucherQty,
          totalValues: this.newVoucherQty * voucher.denominations,
          voucherBudgetDetailId: this.item.id
        }

        this.item.distributeLines.push(newItem);
      } else {
        line.countNumberPage += this.newVoucherQty;
        line.totalValues = line.countNumberPage * voucher.denominations;
      }
      this.dataSource = new MatTableDataSource<E_VoucherBudgetDetailLine>(this.item.distributeLines);
    }
  }

  okClick() {
    if (this.item.distributeLines && this.item.distributeLines.length > 0) {
      // kiem tra tong tien so voi ngan sach
      if (asEnumerable(this.item.distributeLines).Sum(x => x.totalValues) !== this.item.budget) {
        alert('Tổng số tiền không bằng ngân sách');
        return;
      }

      this.item.status = 2;
      this.item.statusName = 'Đã phân bổ';
    }
    this.dialogRef.close(this.item);
  }

  remove(item) {
    const idx = this.item.distributeLines.findIndex(x => x.voucherTypeCode === item.voucherTypeCode);
    // kiem tra voucher da có trong danh sach chua
    if (idx !== -1) {
      this.item.distributeLines.splice(idx, 1);
      this.dataSource = new MatTableDataSource<E_VoucherBudgetDetailLine>(this.item.distributeLines);
    }
  }

  cancelClick() {
    this.dialogRef.close();
  }
}
