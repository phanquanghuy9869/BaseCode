import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { RespondData} from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";
import { map } from 'rxjs/operators';
import { EvoucherCodeFilterModel, EvoucherCodeLineFilterModel } from "src/app/models/base/evoucher";
import { E_VoucherCodeLine } from "src/app/models/data/evoucher";

@Injectable({
    providedIn: 'root'
})
export class EvoucherCodeLineService extends BaseGridService<E_VoucherCodeLine, EvoucherCodeLineFilterModel> {

    private rootUrl = AppConfig.settings.apiServerUrl;
    getAllUrl: string;
    getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.get;
   
    pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.paging;
    addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.addOrEdit;
    deleteUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.delete;
    countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.count;

    constructor(httpClient: HttpClient) {
        super(httpClient);
    }
}