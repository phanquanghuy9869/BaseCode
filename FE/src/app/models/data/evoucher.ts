import { DiaryCellTypeEnum } from "src/app/models/base/utilities";

export interface E_VoucherCode {
    Id?: number;
    CompanyId?: number;
    CompanyName?: string;
    Status?: number;
    StatusName?: string;
    Budget?: number;
    TotalValues?: number;
    VoucheCodeDate?: Date;
    VoucheCodeMonth?: number;
    VoucheCodeYear?: number;
    CreateDate?: Date;
    CreateUser?: string;
    UpdateDate?: Date;
    UpdateUser?: string;
}
export interface E_VoucherCodeLine {
    Id?: number;
    VoucherCodeId?: number;
    EvoucheBudgetDetailId?: number;
    VoucherTypeCode?: string;
    VoucheTypeName?: string;
    Denominations?: number;
    Code?: string;
    StartDate?: Date;
    EndDate?: Date;
    CreateDate?: Date;
    CreateUser?: string;
    UpdateDate?: Date;
    UpdateUser?: string;
    Status?: number;
    StatusName?: string;
    UseDate?: Date;
    Location?: string;
}

export interface E_VoucherCodeDenominations {
    Id?: number;
    VoucherCodeId?: number;
    VoucherTypeName?: string;
    VoucherTypeCode?: string;
    Denominations?: number;
    CountNumber?: number;
    TotalValues?: number;
}

export interface E_VoucherError {
    Id?: number;
    VoucheBudgetId?: number;
    VoucheBudgetDetailId?: number;
    Description?: string;
    VoucheCodeId?: number;
    VoucheCodeLineId?: number;
}

export interface EVoucherViewModel {
    emp?: string;
    data?: string;
    startDate?: Date;
    endDate?: Date;
    price?: number;
}

export interface  View_EVoucherUser {
        id?: number;
        voucherCodeId?: number;
        evoucheBudgetdDetailId?: number;
        voucherTypeCode?: string;
        voucheTypeName?: string;
        denominations?: number;
        code?: string;
        startDate?: Date;
        endDate?: Date;
        createDate?: Date;
        createUser?: string;
        updateDate?: Date;
        updateUser?: string;
        status?: number;
        statusName?: string;
        useDate?: Date;
        location?: string;
        codeUser?: string;
        nameUser?: string;
    }
