import { Component, OnInit, Inject } from '@angular/core';
import { EVoucherCodeImportModel } from '../../../../models/base/utilities';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-e-voucher-import-success',
  templateUrl: './e-voucher-import-success.component.html',
  styleUrls: ['./e-voucher-import-success.component.css']
})
export class EVoucherImportSuccessComponent implements OnInit {  
  importModel: EVoucherCodeImportModel = {};

  constructor(public dialogRef: MatDialogRef<EVoucherImportSuccessComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.importModel = this.data.model;
  }

}
