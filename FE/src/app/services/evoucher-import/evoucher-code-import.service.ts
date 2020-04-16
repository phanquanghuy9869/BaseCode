import { Injectable } from '@angular/core';
import { E_VoucherCodeLine } from 'src/app/models/data/evoucher';
import asEnumerable from 'linq-es2015';
import { EVoucherCodeImportModel } from '../../models/base/utilities';
import { AppConfig } from '../config/app.config';
import { BaseDataService } from 'src/app/services/base/base-data-service';
import { HttpClient } from '@angular/common/http';
import { EVoucherImportSuccessComponent } from '../../modules/share/dialogs/e-voucher-import-success/e-voucher-import-success.component';
import { MatDialog } from '@angular/material';
import { EVoucherImportFailComponent } from 'src/app/modules/share/dialogs/e-voucher-import-fail/e-voucher-import-fail.component';

@Injectable({
  providedIn: 'root'
})
export class EvoucherCodeImportService extends BaseDataService {
  // public dataCols = ['MẪU IMPORT MÃ E-VOUCHER', '__EMPTY', '__EMPTY_1', '__EMPTY_2', '__EMPTY_3'];
  private checkExistedCodeUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeImportUrls.checkIfVoucherCodeExist;
  private importVoucherModelUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeImportUrls.importVoucherModel;

  constructor(protected httpClient: HttpClient,public dialog: MatDialog) {
    super(httpClient);
  }

  async checkExistedCode(model) {
    return this.post(this.checkExistedCodeUrl, model);
  }

  importVoucherModel(model) {
    return this.post(this.importVoucherModelUrl, model);
  }

  async alertImportSuccess(EVoucherCodeImportModel): Promise<boolean> {

    const dialogRef = this.dialog.open(EVoucherImportSuccessComponent, {
      width: '689px',
      data: { model: EVoucherCodeImportModel }
    })

    return dialogRef.afterClosed().toPromise();
  }

  async alertImportFail(model: any): Promise<boolean> {

    const dialogRef = this.dialog.open(EVoucherImportFailComponent, {
      width: '689px',
      data: { model: model }
    })

    return dialogRef.afterClosed().toPromise();
  }

  // getDataByIndex(index, obj) {
  //   const propertyName = this.dataCols[index - 1];
  //   return obj[propertyName];
  // }

  // extractData(data: any[]): EVoucherCodeImportModel {
  //   let rs = this.extractMetaData(data);
  //   rs.importData = this.extractVoucherCode(data);
  //   return rs;
  // }

  // extractMetaData(data: any[]) {
  //   let rs: EVoucherCodeImportModel = {};
  //   rs.orgName = data[1][4];
  //   rs.voucherType = data[2][4];
  //   rs.denominations = data[3][4];
  //   rs.totalNumber = data[4][4];
  //   return rs;
  // }

  // extractVoucherCode(data: any[]): E_VoucherCodeLine[] {
  //   let rs: E_VoucherCodeLine[] = [];
  //   for (let i = 6; i < data.length; i++) {
  //     const el = data[i];
  //     const eVoucher: E_VoucherCodeLine = {Code: el[1], StartDate: new Date(el[2]), EndDate: new Date(el[3]), UseDate: new Date(el[4])}
  //     rs.push(eVoucher);
  //     console.log(eVoucher);
  //   }
  //   return rs;
  // }  
}
