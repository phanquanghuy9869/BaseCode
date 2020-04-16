import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../config/app.config";
import { RespondData} from "src/app/models/base/utilities";
import { BaseGridService } from "../base/base-grid-services";
import { map } from 'rxjs/operators';
import { EvoucherCodeFilterModel,  EvoucherUserCodeFilterModel } from "src/app/models/base/evoucher";
import { View_EVoucherUser } from "src/app/models/data/evoucher";
import { MatDialog } from "@angular/material";
import { AddDiaryDialogComponent } from "src/app/modules/share/dialogs/add-diary-dialog/add-diary-dialog.component";

@Injectable({
    providedIn: 'root'
})
export class EvoucherUserCodeService extends BaseGridService<View_EVoucherUser, EvoucherUserCodeFilterModel> {

    private rootUrl = AppConfig.settings.apiServerUrl;
    getAllUrl: string;
    getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.get;
   
    pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.paging;
    addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.addOrEdit;
    deleteUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.delete;
    countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.evoucherUserCodeUrls.count;

    constructor(httpClient: HttpClient, public dialog: MatDialog) {
        super(httpClient);
    }

    async addDiary(emp, editedDate, diary): Promise<boolean> {

        const dialogRef = this.dialog.open(AddDiaryDialogComponent, {
            width: '689px',
            data: { emp: emp, date: editedDate, eventDiary: diary }
        })

        return dialogRef.afterClosed().toPromise();
    }
}