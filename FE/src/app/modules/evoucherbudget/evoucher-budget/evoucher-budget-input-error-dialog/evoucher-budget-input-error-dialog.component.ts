import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatTableDataSource } from '@angular/material';
import { E_VoucherBudgetDetail, ImportDataErrorRow } from 'src/app/models/data/data';

@Component({
  selector: 'app-evoucher-budget-input-error-dialog',
  templateUrl: './evoucher-budget-input-error-dialog.component.html',
  styleUrls: ['./evoucher-budget-input-error-dialog.component.css']
})
export class EvoucherBudgetInputErrorDialogComponent implements OnInit {

  displayedColumns = ['STT', 'codeUser', 'nameUser', 'jobTitle', 'orgCompanyId', 'budget', 'message'];
  dataSource: MatTableDataSource<ImportDataErrorRow>;

  constructor(public dialogRef: MatDialogRef<EvoucherBudgetInputErrorDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public data: any) {
    this.dataSource = new MatTableDataSource<ImportDataErrorRow>(data.errors);

  }

  ngOnInit() {
  }

  okClick() {
    this.dialogRef.close();
  }

}
