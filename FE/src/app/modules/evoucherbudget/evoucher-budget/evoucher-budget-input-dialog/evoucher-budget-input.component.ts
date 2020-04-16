import { Component, OnInit, Inject } from '@angular/core';
import { E_VoucherBudgetDetail, E_VoucherBudget, Org_Organization } from 'src/app/models/data/data';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-evoucher-budget-input',
  templateUrl: './evoucher-budget-input.component.html',
  styleUrls: ['./evoucher-budget-input.component.css']
})
export class EvoucherBudgetInputDialogComponent implements OnInit {

  result: E_VoucherBudget;
  totalStaff: number;
  totalBudget: number;
  fileData: E_VoucherBudgetDetail[];
  companies: Org_Organization[];
  startDataLine = 2;

  filterDate = new Date();
  filterCompanyId: number;

  constructor(public dialogRef: MatDialogRef<EvoucherBudgetInputDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public data: any) {
    this.companies = data.companies;
  }

  ngOnInit() {
  }

  okClick() {
    if (!this.filterDate) {
      alert('Chưa nhập ngày!');
      return;
    }

    this.result = {
      totalStaff: this.totalStaff
      , budget: this.totalBudget
      , budgetDate: this.filterDate
      , budgetDetails: this.fileData
    };
    this.dialogRef.close(this.result);
  }

  cancelClick() {
    this.dialogRef.close();
  }

  prepareData(data) {
    this.fileData = [];
    this.totalStaff = 0;
    this.totalBudget = 0;
    if (data && data.length > this.startDataLine - 1) {
      for (let i = this.startDataLine - 1; i < data.length; i++) {
        const element = data[i];
        const line = {
          orderNo: +element[0],
          codeUser: element[1],
          nameUser: element[2],
          budget: +element[3],
        };
        this.fileData.push(line);
        this.totalStaff++;
        this.totalBudget += line.budget;
      }
    }
  }
}
