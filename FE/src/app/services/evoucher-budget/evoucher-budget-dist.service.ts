import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { E_VoucherType, E_VoucherBudget, E_VoucherBudgetDetail } from "src/app/models/data/data";
import { RespondData, EvoucherTypeFilterModel, EvoucherBudgetFilterModel } from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";

import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class EvoucherBuggetDistributeService extends BaseDataService {

    getCompaniesUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetUrls.getCompanies;

    findBudgetDistributesUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.findBudgetDistributes;
    saveCompanyBudgetDistUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.saveCompanyBudgetDist;
    saveEmployeeBudgetDistUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.saveEmployeeBudgetDist;
    getCompanyBudgetDistUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.getCompanyBudgetDist;
    getEmployeeBudgetDistUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.getEmployeeBudgetDist;
    getDetailsUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.getDetails;
    getDenominationsUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.getDenominations;
    completeDistributeUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.completeDistribute;
    approveDistributeUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.approveDistribute;
    returnDistributeUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherBudgetDistUrls.returnDistribute;

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }

    findBudgetDistributes(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.findBudgetDistributesUrl, filter);
    }

    getDetails(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.getDetailsUrl, filter);
    }

    getCompanyBudgetDist(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.getCompanyBudgetDistUrl, filter);
    }

    getEmployeeBudgetDist(filter: EvoucherBudgetFilterModel): Promise<RespondData> {
        return this.post(this.getEmployeeBudgetDistUrl, filter);
    }

    getDenominations(): Promise<RespondData> {
        return this.post(this.getDenominationsUrl, {});
    }

    saveCompanyBudgetDist(filter: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.saveCompanyBudgetDistUrl, filter);
    }

    saveEmployeeBudgetDist(filter: E_VoucherBudgetDetail): Promise<RespondData> {
        return this.post(this.saveEmployeeBudgetDistUrl, filter);
    }

    completeDistribute(filter: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.completeDistributeUrl, filter);
    }

    approveDistribute(filter: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.approveDistributeUrl, filter);
    }

    returnDistribute(filter: E_VoucherBudget): Promise<RespondData> {
        return this.post(this.returnDistributeUrl, filter);
    }
}