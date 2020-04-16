import { Component, OnInit } from '@angular/core';
import { RespondData, UserImportModel } from 'src/app/models/base/utilities';
import { Org_Organization, E_VoucherType } from 'src/app/models/data/data';
import { UserOrgService } from 'src/app/services/orgs/user-org/user-org.service';
import { EvoucherTypeService } from 'src/app/services/evoucher-budget/evoucher-type.service';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import asEnumerable from 'linq-es2015';
import { RegexHelper } from '../../../helpers/regex-validate-helper';
import { invalid } from '@angular/compiler/src/render3/view/util';
import { Location } from '@angular/common';

export interface UserImportData {
  userFullName?: string;
  dob?: Date;
  userEmail?: string;
  phoneNumber?: string;
  identity?: string;
  message?: string;
}

@Component({
  selector: 'app-user-import',
  templateUrl: './user-import.component.html',
  styleUrls: ['./user-import.component.css']
})
export class UserImportComponent implements OnInit {
  importModel: UserImportModel = {};
  Orgs: Org_Organization[] = [];
  excelColMapping = ['userFullName', 'dob', 'userEmail', 'phoneNumber', 'identity'];

  constructor(private _userOrgService: UserOrgService, private _voucherTypeService: EvoucherTypeService, private _dialogService: CommonDialogService,
    private _location: Location) { }

  ngOnInit() {
    this.initializeData();
  }

  import(data: any[]) {
    // this.importModel = this._voucherImportService.extractData(data);
    console.log(data);
    this.importModel.importData = data;
    if (this.importModel.importData != null && this.importModel.importData.length > 0) {
      this.importModel.importData.shift();
    }
    for (let i = 0; i < this.importModel.importData.length; i++) {
      const el = this.importModel.importData[i];
      el.dob = el.dob == null ? null : new Date(el.dob);
      // el.EndDate = el.EndDate == null ? null : new Date(el.EndDate);
      // el.UseDate = el.UseDate == null ? null : new Date(el.UseDate);
    }
    console.log('Import data: ', this.importModel.importData);
  }

  async validateData() {
    let isValid: RespondData = { isSuccess: false, message: '', status: '', data: [] };
    if (this.importModel.orgId == null || this.importModel.empCount == null) {
      isValid.message = 'Dữ liệu công ty, số lượng nhân viên không được để trống!';
      isValid.status = 'input';
      return isValid;
    }

    if (this.importModel.importData == null || this.importModel.importData.length == 0) {
      isValid.message = 'Không có dữ liệu nào được import!';
      isValid.status = 'input';
      return isValid;
    }

    if (this.importModel.empCount != this.importModel.importData.length) {
      isValid.message = 'Số lượng nhân viên không khớp!';
      isValid.status = 'input';
      return isValid;
    }

    const invalidEmails = asEnumerable(this.importModel.importData).Where(x => !RegexHelper.isValidEmail(x.userEmail)).ToArray();

    if (invalidEmails != null && invalidEmails.length > 0) {
      for (let i = 0; i < invalidEmails.length; i++) {
        const el = invalidEmails[i];
        el.message = 'Email không hợp lệ';
      }
      isValid.data.push(...invalidEmails);
    }

    const invalidPhones = asEnumerable(this.importModel.importData).Where(x => !RegexHelper.isValidPhoneNumber(x.phoneNumber)).ToArray();

    if (invalidPhones != null && invalidPhones.length > 0) {
      for (let i = 0; i < invalidPhones.length; i++) {
        const el = invalidPhones[i];
        el.message = 'Phone không hợp lệ';
      }
      isValid.data.push(...invalidPhones);
    }

    const checkExistedPhoneResponse = await this._userOrgService.checkExistedPhone(asEnumerable(this.importModel.importData).Select(x => x.phoneNumber).ToArray());
    if (checkExistedPhoneResponse.isSuccess) {
      if (checkExistedPhoneResponse.data != null && checkExistedPhoneResponse.data.length != 0) {
        isValid.status = 'import';
        const duplicates = asEnumerable(this.importModel.importData).Where(x => (checkExistedPhoneResponse.data as any[]).includes(x.phoneNumber)).ToArray();
        for (let i = 0; i < duplicates.length; i++) {
          const el = duplicates[i];
          el.message = 'Số điện thoại đã tồn tại';
        }
        (isValid.data as any[]).push(...duplicates);
        return isValid;
      }
    } else {
      isValid.message = 'Có lỗi xảy ra.';
      isValid.status = 'input';
      return isValid;
    }

    isValid.isSuccess = isValid.data.length == 0;
    return isValid;
  }

  fetchImportAppData() {
  }

  initializeData() {
    this._userOrgService.getOrgs().then(
      (result) => {
        this.Orgs = [];
        result.data.forEach(e => {
          let org = {
            id: e.id, name: e.name, description: e.description, organizationTypeID: e.organizationTypeID,
            nodeID: e.nodeID, directoryPath: e.directoryPath
          };
          this.Orgs.push(org);
        });
      }
      ,
      (error) => {
        console.log(error);
      }
    );
  }

  async submit() {
    try {
      const isValid = await this.validateData();

      if (!isValid.isSuccess && (isValid.status == 'input')) {
        this._dialogService.alert(isValid.message);
        return;
      }

      this.fetchData();
      if (!isValid.isSuccess && (isValid.status == 'import')) {
        const invalidRows = isValid.data as UserImportData[];
        const invalidRowsPhone = asEnumerable(invalidRows).Select(x => x.phoneNumber).ToArray();
        const validRows = asEnumerable(this.importModel.importData).Where(x => !invalidRowsPhone.includes(x.phoneNumber)).ToArray();

        // no valid row
        if (validRows == null || validRows.length == 0) {
          this.alertInvalidData(validRows, invalidRows);
          return;
        }

        this.importModel.importData = validRows;
        const rs = await this.importData(invalidRows);
        return;
      }

      if (isValid.isSuccess) {
        this.importData();
      }

    } catch (error) {
      this.alertUnknownError();
    }
  }

  async importData(invalidRows: any[] = []) {
    const rs = await this._userOrgService.importUser(this.importModel);
    if (!rs.isSuccess) {
      this.alertUnknownError();
      return;
    }

    if (rs.data != null && rs.data.length > 0) {
      const unableImportPhone = rs.data as string[];
      const unableImportRows = asEnumerable(this.importModel.importData).Where(x => unableImportPhone.includes(x.phoneNumber)).ToArray();
      for (let i = 0; i < unableImportRows.length; i++) {
        const row = unableImportRows[i];
        row.message = 'Lỗi không xác định.';
      }
      invalidRows.push(...unableImportRows);
    }

    if (invalidRows.length == 0) {
      this._userOrgService.alertImportSuccess({ importModel: this.importModel });
      return;
    }

    const invalidRowsPhone = asEnumerable(invalidRows).Select(x => x.phoneNumber).ToArray();
    const validRow = asEnumerable(this.importModel.importData).Where(x => !invalidRowsPhone.includes(x.phoneNumber)).ToArray();
    await this.alertInvalidData(validRow, invalidRows);
  }

  fetchData() {
    this.importModel.orgName = asEnumerable(this.Orgs).FirstOrDefault(x => x.id == this.importModel.orgId).name;
  }

  alertUnknownError() {
    this._dialogService.alert('Có lỗi xảy ra!');
  }

  alertInvalidData(validRows: UserImportData[], invalidRows: UserImportData[]) {
    this._userOrgService.alertImportFail({ validRows: validRows, invalidRows: invalidRows, importModel: this.importModel });
  }


  back() {
    this._location.back();
  }
}
