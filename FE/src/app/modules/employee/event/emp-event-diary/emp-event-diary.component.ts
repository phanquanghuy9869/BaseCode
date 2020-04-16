import { DatePipe } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DateAdapter, MatDatepicker, MatTable, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import asEnumerable from 'linq-es2015';
// import { Moment } from 'moment';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { DateHelper } from '../../../../helpers/date-helper';
import { DiaryCellTypeEnum, EventDiaryFilterModel } from '../../../../models/base/utilities';
import { DiaryCriterionDetail, DiaryDisplayCell, EmpModel, EventDiaryDisplayModel, EventDiaryModel, EventDiaryConfig, Org_Organization, Org, UserOrg } from '../../../../models/data/data';
import { EventService } from '../../../../services/event-diary/event.service';
import { CommonDialogService } from '../../../../services/utilities/dialog/dialog.service';
import { UserOrgService } from 'src/app/services/orgs/user-org/user-org.service';
import { EventDiaryConfigService } from 'src/app/services/event-diary/event-diary-config.service';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BaseMngEventDiary } from 'src/app/models/base/base-mng-event-diary';

const moment = _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-emp-event-diary',
  templateUrl: './emp-event-diary.component.html',
  styleUrls: ['./emp-event-diary.component.css'], providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})

export class EmpEventDiaryComponent extends BaseMngEventDiary {
  moduleName = 'EmpEventDiaryComponent';

  orgs: Org[] = [];

  constructor(_service: EventService, _dialogService: CommonDialogService, _datepipe: DatePipe, _userOrgService: UserOrgService,
    _eventDiaryConfigService: EventDiaryConfigService, _authService: AuthService) {
    super(_service, _dialogService, _datepipe, _userOrgService, _eventDiaryConfigService, _authService);
  }

  async ngOnInit() {
    // super.ngOnInit();


    await this.getData();
  }

  async getLevel1MangerName() {
    const res = await this._userOrgService.getLevel1ManagerByUserName(this._authService.getUsername(),
      +this._datepipe.transform(this.diaryDate.value, 'yyyyMM'));
    if (res.isSuccess) {
      const data = res.data as UserOrg;
      if (data) {
        this.managerFullName = data.userFullName;
      }
    } else {
      console.log(res.message);
    }
  }

  async getModel() {
    try {
      await this.getLevel1MangerName();
      // get config org and user
      // const [eventRes, empRes] = await Promise.all([this._userOrgService.getOrgByUser(), this._userOrgService.getEDUsersByOrg(2)]);
      const eventRes = await this._userOrgService.getOrgByUser();
      if (eventRes.isSuccess) {
        if (eventRes.data != null) {
          this.orgs = [eventRes.data];
          this.filter.orgId = this.orgs[0].id;
        }
      } else {
        this._dialogService.alert(eventRes.message);
      }

      await this.searchPaging();
    } catch (err) {
      this._dialogService.alert(err.message);
    }
  }

  getEvent(yearMonth) {
    return this._service.getEventByEmp(yearMonth, this.filter.orgId);
  }

}