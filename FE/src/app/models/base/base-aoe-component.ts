import { GridFilterModel, RespondData } from "./utilities";

import { IGridService } from "../../services/base/base-grid-services";
import { OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { CommonDialogService } from "src/app/services/utilities/dialog/dialog.service";
import { appGlobals } from "src/app/modules/share/app-global";

export abstract class BaseAddOrEditComponent<TData, TFilter extends GridFilterModel, TService extends IGridService<TData, TFilter>> implements OnInit {
    id: number;
    item: TData;
    isReadOnly = true;
    public page = 0;

    constructor(protected _dataService: TService, protected _route: ActivatedRoute, protected _router: Router, protected _dialogService: CommonDialogService) {
    }

    async ngOnInit() {
        this.id = this._route.snapshot.params['id'];
        if (this._route.snapshot.params['page']) {
            this.page = this._route.snapshot.params['page'];
        }

        await this.customInit();
        const response = await this._dataService.get(this.id);
        if (response.isSuccess) {
            this.item = response.data;
            console.log(this.item);
            this.isReadOnly = this._dataService.getReadOnlyStatus(this.getReadOnlyFilter());
        }
    }

    async customInit() {
    }

    getReadOnlyFilter() {
        return null;
    }

    async addOrEdit() {
        const isValid = this.validateData(this.item);

        if (!isValid.isSuccess) {
            console.log(isValid.message);
            await this._dialogService.alert('Đã có lỗi xảy ra/Error!');
        }

        try {
            this.fetchData();
            const rs = await this._dataService.addOrEdit(this.item);
            if (rs.isSuccess) {
				if (appGlobals.getLang()=='vn'){
					await this._dialogService.alert('Thay đổi thành công!');
				}else{
					await this._dialogService.alert('Successful Changed!');
				}
            } else {
                console.log(rs.message);
                await this._dialogService.alert('Đã có lỗi xảy ra/Error!');
            }
        } catch (err) {
            alert(err.message);
        }
    }

    abstract validateData(input: TData): RespondData;
    abstract fetchData();
}