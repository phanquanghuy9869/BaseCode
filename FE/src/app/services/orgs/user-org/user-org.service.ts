import { Injectable } from "@angular/core";
import { BaseDataService } from "src/app/services/base/base-data-service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AppConfig } from "../../config/app.config";
import { UserOrg, DivMngPerUser } from "src/app/models/data/data";
import { UserOrgFilterModel, RespondData } from "src/app/models/base/utilities";
import { BaseGridService } from "../../base/base-grid-services";

import { map } from 'rxjs/operators';
import { MatDialog } from "@angular/material";
import { EVoucherImportSuccessComponent } from "../../../modules/share/dialogs/e-voucher-import-success/e-voucher-import-success.component";
import { EVoucherImportFailComponent } from "../../../modules/share/dialogs/e-voucher-import-fail/e-voucher-import-fail.component";
import { UserImportSuccessComponent } from "src/app/modules/share/dialogs/user-import-success/user-import-success.component";
import { UserImportFailComponent } from "src/app/modules/share/dialogs/user-import-fail/user-import-fail.component";

@Injectable({
    providedIn: 'root'
})
export class UserOrgService extends BaseGridService<UserOrg, UserOrgFilterModel> {

    private rootUrl = AppConfig.settings.apiServerUrl;
    getAllUrl: string;
    getUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.get;
    getLevel1ManagerByUserNameUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getLevel1ManagerByUserName;

    pagingUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.paging;
    addOrEditUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.addOrEdit;
    deleteUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.deleteUser;
    countUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.count;
    orgsUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.orgs;
    usersUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.users;
    jobTitlesUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.jobTitles;
    private getEDUsersByOrgUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getEDUsersByOrg;
    private getOrgByUserUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getOrgByUser;
    private getEDUsersByEventUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getEDUsersByEvent;
    private getEDUsersByEventsUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getEDUsersByEvents;
    private getOrgByCurrentLv2MngUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getOrgsByCurrentLevel2Manager;
    private getOrgByCurrentLv1MngUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getOrgsByCurrentLevel1Manager;
    private getLevel1ManagerUserOrgUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getLevel1ManagerUserOrg;
    private getLevel2ManagerUserOrgUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getLevel2ManagerUserOrg;
    private countDivisionManagerUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.countDivisionManager;
    private searchPagingDivisionManagerUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.searchPagingDivisionManager;
    private getDivisionManagerPermissionUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.getDivisionManagerPermission;
    private saveDivisionManagerPermissionUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.saveDivisionManagerPermission;
    private validateVipUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.isVip;

    private importUserUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.importUser;
    private checkExistedPhoneNumberUrl = AppConfig.settings.apiServerUrl + AppConfig.settings.userOrgUrls.checkExistedPhoneNumber;

    constructor(httpClient: HttpClient, public dialog: MatDialog) {
        super(httpClient);
    }

    getEDUsersByOrg(orgId: number) {
        return this.post(this.getEDUsersByOrgUrl, { orgId: orgId });
    }

    getOrgs() {
        return this.post(this.orgsUrl, '');
    }

    getUsers() {
        return this.post(this.usersUrl, '');
    }
    getJobTitles() {
        return this.post(this.jobTitlesUrl, '');
    }

    getLevel1ManagerByUserName(name: string, yearMonth: number) {
        const data = { userName: name, yearMonth: yearMonth };
        return this.post(this.getLevel1ManagerByUserNameUrl, data);
    }

    getOrgByUser() {
        return this.post(this.getOrgByUserUrl, {});
    }

    getOrgByCurrentLv2Mng() {
        return this.post(this.getOrgByCurrentLv2MngUrl, {});
    }

    getOrgByCurrentLv1Mng() {
        return this.post(this.getOrgByCurrentLv1MngUrl, {});
    }

    getEDUserByEvent(eventId) {
        return this.post(this.getEDUsersByEventUrl, { eventDiaryId: eventId });
    }

    getEDUserByEvents(eventIds) {
        console.log('Event ids: ', eventIds);
        return this.post(this.getEDUsersByEventsUrl, { eventDiaryIds: eventIds });
    }

    getOrgsEx() {
        return this.httpClient.post<any>(`${this.orgsUrl}`, {})
            .pipe(map(ret => {
                return ret;
            }));
    }

    getLevel1ManagerUserOrg() {
        console.log(this.getLevel1ManagerUserOrgUrl);
        return this.post(this.getLevel1ManagerUserOrgUrl, {});
    }

    getLevel2ManagerUserOrg() {
        console.log(this.getLevel2ManagerUserOrgUrl);
        return this.post(this.getLevel2ManagerUserOrgUrl, {});
    }

    RemoveUser(userName: string) {
        return this.post(this.deleteUrl, { userName: userName });
    }

    countDivisionManager(filter: UserOrgFilterModel) {
        return this.post(this.countDivisionManagerUrl, filter);
    }

    searchPagingDivisionManager(filter: UserOrgFilterModel) {
        return this.post(this.searchPagingDivisionManagerUrl, filter);
    }

    getDivisionManagerPermission(id: number) {
        return this.post(`${this.getDivisionManagerPermissionUrl}/${id}`, {});
    }

    saveDivisionManagerPermission(model: DivMngPerUser) {
        return this.post(`${this.saveDivisionManagerPermissionUrl}`, model);
    }

    validateVip(): Promise<RespondData> {
        return this.post(this.validateVipUrl, {});
    }

    importUser(model): Promise<RespondData> {
        return this.post(this.importUserUrl, model);
    }

    checkExistedPhone(model): Promise<RespondData> {
        return this.post(this.checkExistedPhoneNumberUrl, model);
    }

    async alertImportSuccess(model): Promise<boolean> {
        const dialogRef = this.dialog.open(UserImportSuccessComponent, {
            width: '669px',
            data: { model: model }
        })

        return dialogRef.afterClosed().toPromise();
    }

    async alertImportFail(model: any): Promise<boolean> {
        const dialogRef = this.dialog.open(UserImportFailComponent, {
            width: '986px',
            data: { model: model }
        })

        return dialogRef.afterClosed().toPromise();
    }

}