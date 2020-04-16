import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UserImportModel } from 'src/app/models/base/utilities';

@Component({
  selector: 'app-user-import-success',
  templateUrl: './user-import-success.component.html',
  styleUrls: ['./user-import-success.component.css']
})
export class UserImportSuccessComponent implements OnInit {
  validRows: any[] = [];
  importModel: UserImportModel;

  constructor(public dialogRef: MatDialogRef<UserImportSuccessComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.importModel = this.data.model.importModel;
  }
}
