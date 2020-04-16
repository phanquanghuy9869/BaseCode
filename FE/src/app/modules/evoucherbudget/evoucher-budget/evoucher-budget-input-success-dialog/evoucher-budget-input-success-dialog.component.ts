import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-evoucher-budget-input-success-dialog',
  templateUrl: './evoucher-budget-input-success-dialog.component.html',
  styleUrls: ['./evoucher-budget-input-success-dialog.component.css']
})
export class EvoucherBudgetInputSuccessDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<EvoucherBudgetInputSuccessDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public data: any) {

  }

  ngOnInit() {
  }

  okClick() {
    this.dialogRef.close();
  }
}
