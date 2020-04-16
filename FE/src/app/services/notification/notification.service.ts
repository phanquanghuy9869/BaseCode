import { Injectable } from '@angular/core';
import { AppConfig } from '../config/app.config';
import { KpiNotificationFilterModel } from 'src/app/models/base/utilities';
import { KpiNotification } from 'src/app/models/data/data';
import { BaseGridService } from '../base/base-grid-services';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseGridService<KpiNotification, KpiNotificationFilterModel>{
  getAllUrl: string;
  getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.get;
  pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.paging;
  addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.addOrEdit;
  deleteUrl: string;
  countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.count;
  setNotificationStatusUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.setNotificationStatus;
  countNewUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.kpiNotificationUrls.countNew;

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
  }

  markRead(id: number) {
    return this.post(this.setNotificationStatusUrl, { id: id, status: 1 });
  }

  markUnRead(id: number) {
    return this.post(this.setNotificationStatusUrl, { id: id, status: 0 });
  }

  countNew() {
    return this.post(this.countNewUrl, {});
  }
}
