import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UserImportData } from '../../../user/user-import/user-import.component';
import { UserImportModel } from '../../../../models/base/utilities';

@Component({
  selector: 'app-user-import-fail',
  templateUrl: './user-import-fail.component.html',
  styleUrls: ['./user-import-fail.component.css']
})
export class UserImportFailComponent implements OnInit {
  errors: string[];
  displayCol = ['#', 'userFullName', 'phoneNumber', 'dob', 'userEmail', 'message'];
  validRows: UserImportData[] = [];
  invalidRows: UserImportData[] = [];
  importModel: UserImportModel;

  constructor(public dialogRef: MatDialogRef<UserImportFailComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.validRows = this.data.model.validRows;
    this.invalidRows = this.data.model.invalidRows;
    this.importModel = this.data.model.importModel;
  }
}
