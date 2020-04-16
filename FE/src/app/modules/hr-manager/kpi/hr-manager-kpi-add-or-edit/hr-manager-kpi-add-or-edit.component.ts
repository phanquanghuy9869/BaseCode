import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseAddOrEditComponent } from 'src/app/models/base/base-aoe-component';
import { Kpi, KpiTask, CriterionCatalog, KpiCriterionDetail, EventDiaryModel, EmpModel } from '../../../../models/data/data';
import { KpiFilterModel, RespondData, KpiReadOnlyFilter } from '../../../../models/base/utilities';
import { KpiService } from '../../../../services/kpi/kpi.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonDialogService } from '../../../../services/utilities/dialog/dialog.service';
import { MatTable } from '@angular/material';
import asEnumerable from 'linq-es2015';
import { promise } from 'protractor';
import { EventService } from 'src/app/services/event-diary/event.service';
import { BaseKpiAddOrEditComponent } from 'src/app/models/base/base-kpi-add-or-edit-component';
import { Location } from '@angular/common';
import { appGlobals } from 'src/app/modules/share/app-global';

@Component({
  selector: 'app-hr-manager-kpi-add-or-edit',
  templateUrl: './hr-manager-kpi-add-or-edit.component.html',
  styleUrls: ['./hr-manager-kpi-add-or-edit.component.css']
})
export class HrManagerKpiAddOrEditComponent extends BaseKpiAddOrEditComponent {
  moduleName = 'HrManagerKpiAddOrEditComponent';
  taskDisplayedColumns = ['STT', 'task', 'assignedDate', 'expectation', 'deadline', 'deadlineStr', 'result'];
  criterionDisplayedColumns = ['STT', 'criterionTitle', 'maximumPoint', 'employeeEvaluatePoint', 'employeeEvaluateDate'
    , 'managerEvaluatePoint', 'managerEvaluateDate'];

  constructor(_dataService: KpiService, _route: ActivatedRoute, _router: Router, _dialogService: CommonDialogService,
    _evnService: EventService, _location: Location) {
    super(_dataService, _route, _router, _dialogService, _evnService, _location);
  }

  protected initialize() {
    this.getCriterionDetails();
    this.createCriterionAggregateRow();
    this.calCriterionAggregateRow();

    // tu dong gan diem GD nhan su
    if (!this.item.hrKpiPoint || this.item.hrKpiPoint <= 0) {
      this.item.hrKpiPoint = this.item.level1ManagerKpiPoint;
      this.item.hrKpiPointClassification = this.item.level1ManagerKpiClassification;
    }
  }

  protected updateItem() {
    return this._dataService.updateKpiSendCorLeader(this.item);
  }

  private hrDirectorPointChange() {
    this.item.hrKpiPointClassification = this.calKpiClassification(this.item.hrKpiPoint);
  }

  protected addTaskBuffer() {
  }

  async save() {
    try {
      const rs = await this._dataService.saveKpiSendCorLeader(this.item);

      if (rs.isSuccess === true) {
        if (appGlobals.getLang() == 'vn') {
          await this._dialogService.alert('Lưu thành công.');
        } else {
          await this._dialogService.alert('Save successfully.');
        }
        this.goBack();
      } else {
        await this._dialogService.alert(rs.message);
      }
    } catch (error) {
      console.log(error);
    }
  }

  // getDisplayTask() {
  //   return this.item.taskList;
  // }
}