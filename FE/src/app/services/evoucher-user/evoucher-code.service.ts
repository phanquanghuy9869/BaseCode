import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { RespondData} from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";
import { map } from 'rxjs/operators';
import { EvoucherCodeFilterModel } from "src/app/models/base/evoucher";
import { E_VoucherCode } from "src/app/models/data/evoucher";

@Injectable({
    providedIn: 'root'
})
export class EvoucherCodeService extends BaseGridService<E_VoucherCode, EvoucherCodeFilterModel> {

    private rootUrl = AppConfig.settings.apiServerUrl;
    getAllUrl: string;
    getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeUrls.get;
    getLevel1ManagerByUserNameUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getLevel1ManagerByUserName;

    pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeUrls.paging;
    addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeUrls.addOrEdit;
    deleteUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeUrls.delete;
    countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherCodeUrls.count;

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }
}