import { EventDiary, EmpModel, EventDiaryModel, ProcessStatus } from "src/app/models/data/data";
import { GridFilterModel } from "./utilities";

export interface EvoucherCodeFilterModel extends GridFilterModel {
    name?: string;
    code?: string;
    isValidate?: string;
}

export interface EvoucherCodeLineFilterModel extends GridFilterModel {
    EvoucheBudgetDetailId?: number;
    Status?: number;
    VoucherCodeId?: number;
}
export interface EvoucherUserCodeFilterModel extends GridFilterModel {
    StatusName?: string;
    Status?: number;
    VoucherCode?: string;
    IsExpiryDate?: number;
}