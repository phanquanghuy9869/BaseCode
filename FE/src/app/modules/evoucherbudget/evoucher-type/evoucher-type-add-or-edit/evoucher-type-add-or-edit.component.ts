import { Component, OnInit } from '@angular/core';
import { BaseAddOrEditComponent } from 'src/app/models/base/base-aoe-component';
import { Org_Organization, Org_OrganizationType, E_VoucherType } from 'src/app/models/data/data';
import { OrgService } from 'src/app/services/orgs/org.service';
import { RespondData, OrgFilterModel, EvoucherTypeFilterModel } from 'src/app/models/base/utilities';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import { MatDialog } from '@angular/material';
import { asEnumerable } from 'linq-es2015';
import { appGlobals } from 'src/app/modules/share/app-global';
import { EvoucherTypeService } from 'src/app/services/evoucher-budget/evoucher-type.service';

@Component({
  selector: 'app-evoucher-type-add-or-edit',
  templateUrl: './evoucher-type-add-or-edit.component.html',
  styleUrls: ['./evoucher-type-add-or-edit.component.css']
})
export class EvoucherTypeAddOrEditComponent extends BaseAddOrEditComponent<E_VoucherType, EvoucherTypeFilterModel
, EvoucherTypeService> implements OnInit {
  orgs: Org_Organization[];
  orgTypes: Org_OrganizationType[];

  constructor(_dataService: EvoucherTypeService, _route: ActivatedRoute, _router: Router, _dialogService: CommonDialogService
    , public dialog: MatDialog) {
    super(_dataService, _route, _router, _dialogService);
  }

  async ngOnInit() {
    await super.ngOnInit();

    // tao moi
    if (!this.item) {
      this.item = { id: 0 };
    }
  }

  async addOrEdit() {
    if (!this.item.code) {
      alert('Chưa nhập mã voucher');
      return;
    } else if (this.item.code.trim() === '') {
      alert('Chưa nhập mã voucher');
      return;
    }

    if (!this.item.name) {
      alert('Chưa nhập tên voucher');
      return;
    } else if (this.item.name.trim() === '') {
      alert('Chưa nhập tên voucher');
      return;
    }

    const tmp: E_VoucherType = {
      id: this.item.id,
      code: this.item.code,
      name: this.item.name,
      denominations: this.item.denominations,
      oderNumber: this.item.oderNumber,
      isValidate: this.item.isValidate,
      description: this.item.description
    };
    console.log(tmp);
    this._dataService.addOrEdit(tmp).then(
      (data) => {
        if (data.isSuccess === true) {
          if (appGlobals.getLang() === 'vn') {
            alert('Lưu thành công.');
          } else {
            alert('Save successfully.');
          }
          this._router.navigate(['/ev-budget/ev-type-list/0']);
        } else {
          alert(data.message);
        }
        console.log(data);
      }
      , (error) => {
        console.log(error);
        alert(error);
      }
    );
  }

  validateData(input: E_VoucherType): RespondData {
    return { isSuccess: true };
  }

  fetchData() {
  }
}
