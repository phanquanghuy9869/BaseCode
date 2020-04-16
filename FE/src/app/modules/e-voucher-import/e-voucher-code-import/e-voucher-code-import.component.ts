import { Component, OnInit } from '@angular/core';
import { EVoucherCodeImportModel, RespondData } from 'src/app/models/base/utilities';
import { EvoucherCodeImportService } from 'src/app/services/evoucher-import/evoucher-code-import.service';
import { Org_Organization, E_VoucherType } from 'src/app/models/data/data';
import { UserOrgService } from 'src/app/services/orgs/user-org/user-org.service';
import { EvoucherTypeService } from 'src/app/services/evoucher-budget/evoucher-type.service';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import asEnumerable from 'linq-es2015';
import { E_VoucherCodeLine } from 'src/app/models/data/evoucher';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-e-voucher-code-import',
  templateUrl: './e-voucher-code-import.component.html',
  styleUrls: ['./e-voucher-code-import.component.css']
})
export class EVoucherCodeImportComponent implements OnInit {
  // compareModel: EVoucherCodeImportModel = {};
  importModel: EVoucherCodeImportModel = {};
  Orgs: Org_Organization[] = [];
  voucherTypes: E_VoucherType[] = [];
  excelColMapping = ['VoucherTypeCode', 'Code', 'StartDate', 'EndDate', 'UseDate'];

  constructor(private _voucherImportService: EvoucherCodeImportService, private _userOrgService: UserOrgService, private _voucherTypeService: EvoucherTypeService
    , private _dialogService: CommonDialogService) { }

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
      el.StartDate = el.StartDate == null ? null : new Date(el.StartDate);
      el.EndDate = el.EndDate == null ? null : new Date(el.EndDate);
      el.UseDate = el.UseDate == null ? null : new Date(el.UseDate);
    }
    console.log(this.importModel.importData);
  }

  async validateData() {
    let isValid: RespondData = { isSuccess: false, message: '', status: '' };

    if (this.importModel.time == null || this.importModel.orgId == null || this.importModel.voucherTypeCode == null || this.importModel.denominations == null) {
      isValid.message = 'Dữ liệu thời gian, công ty, loại voucher, mệnh giá không được để trống!';
      return isValid;
    }

    // // validate dữ liệu import
    // if (this.importModel.denominations == null || this.importModel.orgName == null || this.importModel.voucherType == null || this.importModel.totalNumber == null) {
    //   isValid.message = 'Dữ liệu import: tên công ty, loại e-voucher, mệnh giá, tổng số lượng không được để trống!';
    //   return isValid;
    // }

    if (this.importModel.importData == null || this.importModel.importData.length == 0) {
      isValid.message = 'Không có dữ liệu nào được import!';
      return isValid;
    }

    const voucherTypeCode = asEnumerable(this.voucherTypes).FirstOrDefault(x => x.code == this.importModel.voucherTypeCode).code;
    const isInvalidTypeCode = asEnumerable(this.importModel.importData).Where(x => x.Code != this.importModel.voucherTypeCode).Any();
    if (isInvalidTypeCode) {
      isValid.message = 'Dữ liệu import có mã loại voucher không hợp lệ, mã loại voucher hợp lệ: ' + this.importModel.voucherTypeCode;
      return isValid;
    }

    const checkExistedCodeResponse = await this._voucherImportService.checkExistedCode(asEnumerable(this.importModel.importData).Select(x => x.Code).ToArray());
    if (checkExistedCodeResponse.isSuccess) {
      if (checkExistedCodeResponse.data == null || checkExistedCodeResponse.data.length == 0) {
        isValid.isSuccess = true;
        return isValid;
      } else {
        isValid.status = 'existed';
        isValid.data = checkExistedCodeResponse.data;
        return isValid;
      }
    } else {
      isValid.message = 'Có lỗi xảy ra.';
      return isValid;
    }
  }

  fetchImportAppData() {
    if (this.importModel.orgId == null || this.importModel.voucherTypeCode == null) {
      return;
    }

    // this.importModel.orgName = asEnumerable(this.Orgs).FirstOrDefault(x => x.id == this.importModel.orgId).name;
    this.importModel.voucherTypeName = asEnumerable(this.voucherTypes).FirstOrDefault(x => x.code == this.importModel.voucherTypeCode).name;
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

    this._userOrgService.getOrgByUser().then(
      (result) => {
        const currentOrg = result.data;
        this.importModel.orgId = currentOrg.id;
        this.importModel.orgName = currentOrg.name;
      }
      ,
      (error) => {
        console.log(error);
      }
    );

    this._voucherTypeService.getAll().then(
      (result) => {
        this.voucherTypes = result.data;
      },
      (error) => {
        console.log(error);
      }
    )
  }

  async submit() {
    this.fetchImportAppData();
    const isValid = await this.validateData();

    if (!isValid.isSuccess) {
      if (isValid.status == 'existed') {
        const err = 'Mã ' + isValid.data.join() + ' đã tồn tại';
        this._voucherImportService.alertImportFail([err]);
        return;
      } else {
        this._dialogService.alert(isValid.message);
        return;
      }
    } else {
      const response = await this._voucherImportService.importVoucherModel(this.importModel);
      if (response.isSuccess) {
        this._voucherImportService.alertImportSuccess(this.importModel);
      } else {
        this._dialogService.alert("Có lỗi xảy ra");
      }
    }
  }

  vouchTypeChange() {
    
  }
}
