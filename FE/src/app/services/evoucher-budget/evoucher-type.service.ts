import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { E_VoucherType } from "src/app/models/data/data";
import { RespondData, EvoucherTypeFilterModel } from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";

import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class EvoucherTypeService extends BaseGridService<E_VoucherType, EvoucherTypeFilterModel> {
    private rootUrl = AppConfig.settings.apiServerUrl;
    getAllUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.getAll;
    getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.get;
    getLevel1ManagerByUserNameUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getLevel1ManagerByUserName;
    pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.paging;
    addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.addOrEdit;
    deleteUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.delete;
    countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherTypeUrls.count;

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }
}