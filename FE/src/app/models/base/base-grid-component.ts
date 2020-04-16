import { OnInit, ViewChild, Input } from "@angular/core";
import { MatPaginator } from "@angular/material";
import { GridFilterModel, RespondData } from "./utilities";
import { tap } from 'rxjs/operators';
import { IGridService } from "src/app/services/base/base-grid-services";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, BehaviorSubject } from "rxjs";
import { DataSource } from "@angular/cdk/table";
import { appGlobals } from "src/app/modules/share/app-global";
import { ActivatedRoute } from "@angular/router";

export abstract class BaseGridComponent<TResult, TFilter extends GridFilterModel, TService extends IGridService<TResult, TFilter>,
    TDataSource extends IGridDataSource<TResult, TFilter, TService>> implements OnInit {
    dataSource: TDataSource;
    filter: TFilter;
    abstract displayedColumns: string[];
    abstract start = 0;
    abstract length = 0;
    abstract countTotal = 0;
    public pageSizeOptions = [5, 10, 15, 20, 50];
    public page = 0;
    moduleName: string;

    // https://stackoverflow.com/questions/17382143/create-a-new-object-from-type-parameter-in-generic-class
    @ViewChild(MatPaginator) paginator: MatPaginator;
    constructor(protected _service: TService, private sourceConstructor: new (TService) => TDataSource, protected _route: ActivatedRoute = null) {
    }

    get language() {
        return appGlobals.getLang();
    }

    ngOnInit(): void {
        this.dataSource = new this.sourceConstructor(this._service);
        if (this.filter != null) {
            this.requestData(this.filter);
        }else {
            this.getPaging();
        }        

        if (this._route != null) {
            if (this._route.snapshot.params['page']) {
                this.page = +this._route.snapshot.params['page'];
            }
            console.log("Page: ", this.page);
        }
    }   

    // huypq , bổ sung load dữ liệu từ 
    ngAfterViewInit(): void {
        this.paginator.page
            .pipe(
                tap(() => this.getPaging()),
            )
            .subscribe();
    }

    abstract getFilter(): TFilter;

    // huypq modified 11-06-2019
    resetPaging() {
        this.filter.start = 1;
        this.filter.length = this.paginator.pageSize;
        this.paginator.pageIndex = 0;
    }

    getPaging() {
        const filter = this.getFilter();
        this.requestData(filter);
    }

    requestData(filter) {
        this.dataSource.getPaging(this.filter);
        this.count(this.filter);
        this.onPagingQuery(filter)
    }

    // huypq modified 11-06-2019
    searchPaging() {
        const filter = this.getFilter();
        this.resetPaging();
        this.requestData(filter);
    }

    onPagingQuery(filter) {
    }

    // huypq modified 12-06-2019
    onFormSubmit(event) {
        // tslint:disable-next-line:triple-equals
        if (event.keyCode == 13) {
            this.searchPaging();
        }
    }

    async delete(item: TResult) {
        if (confirm('Chắc chắn muốn xóa?')) {
            try {
                const response = await this._service.detele(item);
                if (response.isSuccess) {
                    this.getPaging();
                    if (appGlobals.getLang()=='vn'){
                    //	await this._dialogService.alert('Thay đổi thành công!');
                        alert('Thay đổi thành công!');
					}else{
                    //	await this._dialogService.alert('Successful Changed!');
                        alert('Successful Changed!');
					}
                } else {
                    this.promtMessage(response.message);
                }
            } catch (err) {
                console.log(err.message);
            }
        }
    }

    // Đếm tổng số phần tử của bản
    async count(filter: TFilter) {
        try {
            const response = await this._service.count(filter);
            if (response.isSuccess) {
                this.countTotal = response.data;
            } else {
                console.log(response.message);
            }
        } catch (err) {
            console.log(err.message);
        }
    }

    promtMessage(msg) {
        alert(msg);
    }

    onPaginateChange(event) {
        this.page = event.pageIndex;
    }
}


export interface IGridDataSource<TModel, TFilter extends GridFilterModel
    , TService extends IGridService<TModel, TFilter>> {
    connect(): Observable<TModel[]>;
    disconnect();
    getPaging(filter: TFilter);
    processData(data: TModel[], filter): any;
}

// data source for server paging grid
export class BaseGridDatasource<TModel, TFilter extends GridFilterModel
    , TService extends IGridService<TModel, TFilter>>
    extends DataSource<any> implements IGridDataSource<TModel, TFilter, TService> {
    data = new BehaviorSubject<TModel[]>([]);

    constructor(protected dataService: TService) {
        super();
    }

    connect(): Observable<TModel[]> {
        return this.data.asObservable();
    }

    disconnect() {
        this.data.complete();
    }

    async getPaging(filter: TFilter) {
        try {
            const response = await this.dataService.getPaging(filter);
            if (response.isSuccess) {
                const data = this.processData(response.data, filter);
                this.data.next(data);
                return;
            }
        } catch (error) {
            console.log(error.message);
        }
    }

    public processData(data: TModel[], filter: TFilter): any {
        return data;
    }
}
