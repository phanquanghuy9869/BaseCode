import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { E_VoucherType, E_VoucherBudget } from "src/app/models/data/data";
import { RespondData, EvoucherTypeFilterModel, EvoucherBudgetFilterModel } from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";

import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class EvoucherBuggetService extends BaseDataService {

    getCompaniesUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.getCompanies;

    findUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.find;

    findByIdUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.findById;
    getDetailsPagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.getDetailsPaging;
    addVoucherBudgetUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.addVoucherBudget;
    finishBudgetUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.finishBudget;

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }

    getCompanies(): Promise<RespondData> {
        return this.post(this.getCompaniesUrl, {});
    }

    find(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.findUrl, filter);
    }

    findById(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.findByIdUrl, filter);
    }

    getDetailsPaging(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.getDetailsPagingUrl, filter);
    }

    addVoucherBudget(data: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.addVoucherBudgetUrl, data);
    }

    finishBudget(data: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.finishBudgetUrl, data);
    }
}