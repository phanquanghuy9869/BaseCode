import { BaseGridComponent, IGridDataSource } from "./base-grid-component";
import { GridFilterModel, KpiFilterModel } from "./utilities";
import { IGridService } from "../../services/base/base-grid-services";
import { ActivatedRoute } from "@angular/router";
import { LocalStorageService } from "src/app/services/storage/local-storage.service";
import { Kpi } from "../data/data";
import { KpiService } from "../../services/kpi/kpi.service";
import { MngLv2KpiDataSource } from "../../modules/managerlevel2/kpi/mnglevel2-kpi-list/mnglevel2-kpi-list.component";


export abstract class BaseCacheFilterComponent<TResult, TFilter extends GridFilterModel, TService extends IGridService<TResult, TFilter>,
    TDataSource extends IGridDataSource<TResult, TFilter, TService>> extends BaseGridComponent<TResult, TFilter, TService, TDataSource>
{
    constructor(_service: TService, sourceConstructor: new (TService) => TDataSource, private _storageService: LocalStorageService) {
        super(_service, sourceConstructor);
    }

    ngOnInit(): void {
        this.loadStorageFilter();
        super.ngOnInit();
    }

    loadStorageFilter() {
        this.filter = this._storageService.get(this.moduleName);
        if (this.filter != null) {
            this.start = this.filter.start;
            this.length = this.filter.length;
            this.paginator.pageIndex = this.filter.pageIndex;
            // this.paginator.pageIndex = this.start - 1;
            this.paginator.pageSize = this.length;
        }
    }

    onPagingQuery(filter) {
        console.log('onPagingQuery');
        this.filter.pageIndex = this.paginator.pageIndex;
        this._storageService.store(this.moduleName, this.filter);
    }
}

export abstract class BaseKpiListComponent extends BaseCacheFilterComponent<Kpi, KpiFilterModel, KpiService, MngLv2KpiDataSource>{
   loadStorageFilter() {
       super.loadStorageFilter();
   }
}