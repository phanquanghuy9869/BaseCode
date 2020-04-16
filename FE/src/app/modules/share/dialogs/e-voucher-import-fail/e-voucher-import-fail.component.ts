import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-e-voucher-import-fail',
  templateUrl: './e-voucher-import-fail.component.html',
  styleUrls: ['./e-voucher-import-fail.component.css']
})
export class EVoucherImportFailComponent implements OnInit {
  errors: string[];
  displayCol = ['#', 'err'];

  constructor(public dialogRef: MatDialogRef<EVoucherImportFailComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.errors = this.data.model;
  }

}
