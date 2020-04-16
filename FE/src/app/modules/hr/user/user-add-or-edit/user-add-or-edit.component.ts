import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MonthPickerComponent } from 'src/app/modules/share/components/month-picker/month-picker.component';
import { Org_Organization, UserOrg, Org_JobTitle, Kpi_CriterionType } from 'src/app/models/data/data';
import { UserOrgFilterModel, RespondData } from 'src/app/models/base/utilities';
import { UserOrgService } from 'src/app/services/orgs/user-org/user-org.service';
import { BaseAddOrEditComponent } from 'src/app/models/base/base-aoe-component';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonDialogService } from 'src/app/services/utilities/dialog/dialog.service';
import { Observable } from 'rxjs';

import { from } from 'rxjs';
import { first } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { UserEmploymentHistoryComponent } from '../user-employment-history/user-employment-history.component';
import { AppConfig } from 'src/app/services/config/app.config';
import { asEnumerable } from 'linq-es2015';
import { Kpi_CriterionTypeService } from 'src/app/services/kpi/kpi-criteriontype.service';
import { appGlobals } from 'src/app/modules/share/app-global';
import { DateHelper } from '../../../../helpers/date-helper';

@Component({
  selector: 'app-user-add-or-edit',
  templateUrl: './user-add-or-edit.component.html',
  styleUrls: ['./user-add-or-edit.component.css']
})
export class UserAddOrEditComponent extends BaseAddOrEditComponent<UserOrg, UserOrgFilterModel,
UserOrgService> implements OnInit, AfterViewInit {

  @ViewChild('monthFilter') monthFilter: MonthPickerComponent;
  Orgs: Org_Organization[];
  UserOrgs: UserOrg[];
  JobTitles: Org_JobTitle[];
  kpiTypes: Kpi_CriterionType[];
  workStatus = asEnumerable(AppConfig.settings.workStatus).ToArray();

  constructor(_dataService: UserOrgService, _route: ActivatedRoute, _router: Router, _dialogService: CommonDialogService
    , public dialog: MatDialog, private _critTypeService: Kpi_CriterionTypeService) {
    super(_dataService, _route, _router, _dialogService);
  }

  async ngOnInit() {
    await super.ngOnInit();
    if (!this.item) {
      this.item = { id: 0 };
    }
    this.item.firstKpiDateYM = DateHelper.getYearMonth(new Date(this.item.firstKpiDate));
    console.log(this.item.firstKpiDate);
    console.log("this.item.firstKpiDateYM: ", this.item.firstKpiDateYM);
    this.initialize();
  }

  ngAfterViewInit(): void {
  }

  private initialize() {
    this.getOrgs();
    this.getUsers();
    this.getJobTitles();
    this.getCriterionType();
  }

  getOrgs() {

    this._dataService.getOrgs().then(
      (result) => {
        this.Orgs = [];

        result.data.forEach(e => {
          let org = {
            id: e.id, name: e.name, description: e.description, organizationTypeID: e.organizationTypeID,
            nodeID: e.nodeID, directoryPath: e.directoryPath
          };
          this.Orgs.push(org);
        });
        let allOrg = {
          id: -1, name: 'Tất cả phòng ban', description: '', organizationTypeID: -1,
          nodeID: '', directoryPath: ''
        };
        this.Orgs.unshift(allOrg);
      }
      ,
      (error) => {
        console.log(error);
      }
    );

  }

  getUsers() {
    this._dataService.getJobTitles().then(
      (result) => {
        this.JobTitles = [];
        result.data.forEach(e => {
          let title = {
            id: e.id,
            title: e.title,
            description: e.description
          };
          this.JobTitles.push(title);
        });
      }
      ,
      (error) => {
        console.log(error);
      }
    );
  }

  getJobTitles() {
    this._dataService.getUsers().then(
      (result) => {
        this.UserOrgs = [];
        result.data.forEach(e => {
          let user = {
            id: e.id, userFullName: e.userFullName, jobTitle: e.jobTitle, orgId: e.orgId
          };
          this.UserOrgs.push(user);
        });
      }
      ,
      (error) => {
        console.log(error);
      }
    );
  }

  getCriterionType() {
    this._critTypeService.getAll().then(
      (result) => {
        this.kpiTypes = result.data;
      }
      ,
      (error) => {
        console.log(error);
      }
    );
  }

  async addOrEdit() {
   
    if (!this.item.userName) {
      alert('Chưa nhập tên đăng nhập');
      return;
    } else if (this.item.userName.trim() === '') {
      alert('Chưa nhập tên đăng nhập');
      return;
    }

    if (!this.item.orgId) {
      alert('Chưa nhập phòng ban');
      return;
    } else if (this.item.orgId <= 0) {
      alert('Chưa nhập phòng ban');
      return;
    }

    if (!this.item.userEmail) {
      alert('Chưa nhập email');
      return;
    } else if (this.item.userEmail.trim() === '') {
      alert('Chưa nhập email');
      return;
    }

    
    if (!this.item.isDivisionManager && !this.item.isEmpManager && !this.item.isEmployee && !this.item.isEVoucherDistributor
      && !this.item.isBudgetDistributor && !this.item.isEVoucherManager && !this.item.isLevel2Manager && !this.item.isDistributorApprover) {
      alert('Phải chọn một quyền');
      return;
    }

    const tmp: UserOrg = {
      id: this.item.id,
      userFullName: this.item.userFullName,
      jobTitleId: this.item.jobTitleId,
      orgId: this.item.orgId,
      userEmail: this.item.userEmail,
      status: this.item.status,
      // firstKpiDateYM: this.monthFilter.getYearMonth(),
      kpiType: this.item.kpiType,
      // level1ManagerId: this.item.level1ManagerId,
      // level2ManagerId: this.item.level2ManagerId,
      isEmployee: this.item.isEmployee,
      isEVoucherManager: this.item.isEVoucherManager,
      // isLevel2Manager: this.item.isLevel2Manager,
      isBudgetDistributor: this.item.isBudgetDistributor,
      isEmpManager: this.item.isEmpManager,
      isEVoucherDistributor: this.item.isEVoucherDistributor,
      isDistributorApprover: this.item.isDistributorApprover,
      isCreateUser: this.item.isCreateUser,
      userName: this.item.userName,
      dob: this.item.dob,
      startWorkDate: this.item.startWorkDate,
      code: this.item.code,
      idCardNumber: this.item.idCardNumber,
      idCardDate: this.item.idCardDate,
      idCardLocation: this.item.idCardLocation,
      isDivisionManager: this.item.isDivisionManager,
      isOrgManager: this.item.isOrgManager,
      phoneNumber: this.item.phoneNumber,
      isActived: this.item.isActived != null ? this.item.isActived : false,
    };
    console.log(tmp);
    this._dataService.addOrEdit(tmp).then(
      (data) => {
        if (data.isSuccess === true) {
          if (appGlobals.getLang() == 'vn') {
            alert('Lưu thành công.');
          } else {
            alert('Save successfully.');
          }
          this._router.navigate(['/hr/user']);
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

  validateData(input: UserOrg): RespondData {
    const result: RespondData = { isSuccess: true, message: '' };
    return result;
  }

  fetchData() {
  }

  permissionChanged() {
    if (this.item != null && !this.item.isHasLogin) {
      this.item.isCreateUser = this.item.isEmpManager || this.item.isEmployee || this.item.isEVoucherDistributor || this.item.isEVoucherManager ||
        this.item.isLevel2Manager || this.item.isDistributorApprover || this.item.isBudgetDistributor || this.item.isDivisionManager;
    }
  }

  createUserChanged() {
    if (this.item != null && !this.item.isHasLogin && !this.item.isCreateUser) {
      this.item.isEmpManager = this.item.isEmployee = this.item.isEVoucherDistributor = this.item.isEVoucherManager =
        this.item.isLevel2Manager = this.item.isDistributorApprover = this.item.isBudgetDistributor = this.item.isDivisionManager = false;
    }
  }

  showHistory() {
    const dialogRef = this.dialog.open(UserEmploymentHistoryComponent, {
      width: '720px',
      data: { userId: this.item.id }
    });
  }

  get isReadOnlyUser() {
    return this.item && this.item.id > 0;
  }

  getFirstKpiDateYm() {
    console.log('getFirstKpiDateYm: ', this.item.firstKpiDate);
    if (this.item == null || this.item.firstKpiDate == null) {
      return null;
    }

    return DateHelper.getYearMonth(this.item.firstKpiDate);
  }
}