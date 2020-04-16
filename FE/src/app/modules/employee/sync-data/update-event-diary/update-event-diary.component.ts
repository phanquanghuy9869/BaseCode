import { Component, OnInit, ViewChild } from '@angular/core';
import { DateHelper } from 'src/app/helpers/date-helper';
import { MonthPickerComponent } from '../../../share/components/month-picker/month-picker.component';
import { Org_Organization, UserOrg, User } from 'src/app/models/data/data';
import { UserOrgService } from 'src/app/services/orgs/user-org/user-org.service';
import { MissingConfigComponent } from 'src/app/modules/employee/sync-data/update-event-diary/missing-config/missing-config.component';
import { ExistedConfigComponent } from 'src/app/modules/employee/sync-data/update-event-diary/existed-config/existed-config.component';
import { EventDiaryConfigService } from '../../../../services/event-diary/event-diary-config.service';

@Component({
  selector: 'app-update-event-diary',
  templateUrl: './update-event-diary.component.html',
  styleUrls: ['./update-event-diary.component.css']
})
export class UpdateEventDiaryComponent implements OnInit {
  @ViewChild('monthFilter') monthFilter: MonthPickerComponent;
  @ViewChild('addForm') addForm: MissingConfigComponent;
  @ViewChild('updateForm') updateForm: ExistedConfigComponent;
  yearMonth: number;
  orgs: Org_Organization[];
  level1Mngs: User[];
  
  constructor(private _userOrgService: UserOrgService, private _eventConfigService: EventDiaryConfigService) { }

  ngOnInit() {
    this.yearMonth = this.monthFilter.getYearMonth();
    this.getOrgs();
    this.getLevel1Mng();
  }

  changeYm(ym) {
    console.log('Detect change: ', ym);
    this.yearMonth = ym;
  }

  changeTrigger($event) {
    console.log('ChangeTrigger: ', $event);
    this.searchPagingForm();
  }

  searchPagingForm() {
    this.yearMonth = this.monthFilter.getYearMonth();
    if (!this.addForm || !this.updateForm) {
      return;
    }
    this.addForm.searchPaging();
    this.updateForm.searchPaging();
  }

  async getOrgs() {
    const orgRs = await this._userOrgService.getOrgs();
    if (orgRs.isSuccess) {
      this.orgs = orgRs.data;
      const noSelect: Org_Organization = { id: null, name: ' -------- Chọn -------- '};
      this.orgs.unshift(noSelect);
    }
  }
  
  async getLevel1Mng() {
    const lv1MngRs = await this._eventConfigService.getConfigLevel1Manager();
    if (lv1MngRs.isSuccess) {
      this.level1Mngs = lv1MngRs.data;
      const noSelect: User = { userName: null, name: ' -------- Chọn -------- '};
      this.level1Mngs.unshift(noSelect);
    }
  }
}
