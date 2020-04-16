import { DiaryCellTypeEnum } from "src/app/models/base/utilities";
import { UserDiaryDetailsComponent } from "src/app/modules/share/dialogs/user-diary/user-diary-details/user-diary-details.component";

export interface Menu {
    id: number;
    url: string;
    text: string;
    nameEnglish: string;
    displayOrder: number;
    parrentID: number;
    hasPermission: boolean;
    isDisplayed: boolean;
    isActive: boolean;
    childs?: Menu[];
}

export interface UserOrg {
    id?: number;
    userName?: string;
    userFullName?: string;
    userEmail?: string;
    orgId?: number;
    orgName?: string;
    jobTitle?: string;
    jobTitleId?: number;
    level1ManagerUserName?: string;
    level1ManagerFullName?: string;
    level2ManagerUserName?: string;
    level2ManagerFullName?: string;
    firstKpiDate?: Date;
    firstKpiDateYM?: number;
    status?: string;
    isOrgManager?: boolean;
    eventDiaryConfigId?: number;
    level1ManagerId?: number;
    level2ManagerId?: number;
    kpiType?: string;
    isEmployee?: boolean;
    isEVoucherManager?: boolean;
    isLevel2Manager?: boolean;
    isEmpManager?: boolean;
    isEVoucherDistributor?: boolean;
    isBudgetDistributor?: boolean;
    isDivisionManager?: boolean;
    isDistributorApprover?: boolean;
    isCreateUser?: boolean;
    password?: boolean;
    isHasLogin?: boolean;
    dob?: Date;
    code?: string;
    startWorkDate?: Date;
    edcUpdateDate?: Date;
    idCardNumber?: string;
    idCardDate?: Date;
    idCardLocation?: string;
    secretWord?: string;
    phoneNumber?: string;
    isActived?: boolean;
}

export interface KpiPeriodConfig {
    id: number;
    code?: string;
    monthNumber?: number;
    yearMonth?: number;
    fromDate?: Date;
    toDate?: Date;
    note?: string;
    createdDate?: Date;
    createdByUser?: string;
    isActived?: number;
    isCreateEventDiary?: number;
    criterionTypes?: number[];
    dayStart?: number;
    dayEnd?: number;
    daySendEvalation?: number;
    periodConfig?: string;
}

export interface ViewEventDiaryConfig {
    Id: number;
    Code: string;
    Name: string;
    Description: string;
    OrgId: number;
    OrgName: string;
    Level1ManagerUserName: string;
    Level1ManagerFullName: string;
    Level1ManagerJobTitle: string;
    Level1ManagerOrgName: string;
    Level2ManagerUserName: string;
    Level2ManagerFullName: string;
    Level2ManagerJobTitle: string;
    Level2ManagerOrgName: string;
    FromDate?: Date;
    ToDate?: Date;
    YearMonth: number;
    isActive: number;
}
export interface ViewUserEventDiary {
    Id: number;
    Name: string;
    Description: string;
    OrgId: number;
    OrgName: string;
    Level1ManagerUserName: string;
    Level1ManagerFullName: string;
    Level1ManagerJobTitle: string;
    Level1ManagerOrgName: string;
    Level2ManagerUserName: string;
    Level2ManagerFullName: string;
    Level2ManagerJobTitle: string;
    Level2ManagerOrgName: string;
    CreatedDate?: Date;
    CreatedByUser: string;
    UpdatedDate?: Date;
    UpdatedByUser: string;
    IsDeleted: boolean;
    UserName: string;
    UserFullName: string;
    UserEmail: string;
    JobTitle: string;
    JobTitleId: number;
    Status: string;
}

export interface KpiTask {
    id?: number;
    taskIndex?: number;
    task?: string;
    expectation?: string;
    assignedDate?: Date;
    deadline?: any;
    result?: string;
    isFinish?: boolean;
    kpiEvaluationId?: number;
    createdDate?: Date;
    createdByUser?: string;

    // -------------------------------------
    isUIDeleted?: boolean;
    isUIBuffer?: boolean;
    deadlineStr?: string;
    files?: FileModel[];
}

export interface CriterionCatalog {
    id?: number;
    code?: string;
    criterionTitle?: string;
    minimumPoint?: number;
    maximumPoint?: number;
    parentId?: number;
    isFolder?: boolean;
    categoryId?: number;
    categoryName?: string;
    criterionLevel?: number;
    isMinus?: boolean;
    parentName?: number;
    criterionTitleEn?: string;
}

export interface KpiCriterionDetail {
    id?: number;
    criterionIndex?: number;
    criterionTitle?: string;
    minimumPoint?: number;
    maximumPoint?: number;
    employeeEvaluatePoint?: number;
    employeeEvaluateComment?: string;
    managerEvaluatePoint?: number;
    managerEvaluateComment?: string;
    month?: number;
    year?: number;
    userId?: string;
    orgId?: number;
    kpiEvaluateId?: number;
    kpiCatalogId?: number;
    isDeleted?: boolean;
    createdDate?: Date;
    createdByUser?: string;

    // ----------------------
    employeeLastUpdatedDate?: Date;
    managerLastUpdatedDate?: Date;

    // -----------------------
    employeeEvaluatePointChar?: string;
    managerEvaluatePointChar?: string;
    aggregateRow?: number;
    criterionTitleEn?: string;
}

export interface KpiEvaluation {
    id?: number;
    userId?: string;
    eventDiaryId?: number;
    employeeName?: string;
    employeeJobTitle?: string;
    level1ManagerUserId?: string;
    level1ManagerFullName?: string;
    level1ManagerOrgId?: number;
    level1ManagerOrgName?: string;
    level1ManagerJobTitleId?: number;
    statusId?: number;
    statusName?: string;
    kpiMonth?: Date;
    kpiMonthNumber?: number;
    yearMonth?: number;
    empKpiPoint?: number;
    empKpiClassification?: string;
    level1ManagerKpiPoint?: number;
    level1ManagerKpiClassification?: string;
    finalKpiPoint?: number;
    finalKpiClassification?: string;
    createdDate?: Date;
    createdByUser?: string;
    approvedByLevel1Manager?: Date;
    approvedByLevel1ManagerId?: string;
    approvedByLevel2Manager?: Date;
    approvedByLevel2ManagerId?: string;
    isActived?: boolean;
    isDeleted?: boolean;

    level2ManagerOrgName?: string;
    level2ManagerJobTitle?: string;
    sentHRDate?: Date;
    submitNote?: string;

    // ------------------------
    hrKpiPoint?: number;
    hrKpiPointClassification?: string;
    hrKpiPointComment?: string;

    finalKpiComment?: number;

    level1ManagerJobTitleName?: string;
    criterionTypeName?: string;
    organization?: string;
    organizationId?: number;
    level1ManagerJobTitle?: string;
    level2ManagerFullName: string;
    statusNameEn: string;
    criterionTypeNameEn: string;
    organizationEn: string;
    level1ManagerJobTitleEn: string;
    level1ManagerOrgNameEn: string;
    level2ManagerJobTitleEn: string;
    level2ManagerOrgNameEn: string;
    employeeJobTitleEn: string;
}

export interface Kpi extends KpiEvaluation {
    taskList?: KpiTask[];
    kpiCriterionDetailList?: KpiCriterionDetail[];
    criterionCatalogList?: CriterionCatalog[];

    uiIsSelected: boolean;
}

export interface EventDiary {
    id?: number;
    orgId?: number;
    orgName?: string;
    level1ManagerUserName?: string;
    level1ManagerFullName?: string;
    level2ManagerUserName?: string;
    level2ManagerFullName?: string;
    fromDate?: Date;
    toDate?: Date;
    yearMonth?: number;
}

export interface EventDiarySyncModel extends EventDiary {
    code?: string;
    uiIsSelected?: boolean;
}

export interface DiaryCriterionDetail {
    id?: number;
    criterionCatalogId?: number;
    criterionCatalogCode?: string;
    criterionCatalogName?: string;
    criterionCatalogFolderId?: number;
    criterionDate?: Date;
    criterionDayOfMonth?: number;
    userName?: string;
    userFullName?: string;
    kpiPoint?: number;
    eventDiaryId?: number;
    kpiMonthNumber?: number;
    comment?: string;
    createdDate?: Date;
    createdByUser?: string;
    pointRange?: string;
    cellData?: DiaryDisplayCell;
    createdByUserFullName?: string;
    createdByUserTitle?: string;
    isDeleted?: boolean;
    isMinus?: boolean;
    isLevel1MngAndOwner?: boolean;
    isHrAndOwner?: boolean;
    isHrMngAndOwner?: boolean;
    criterionCatalogNameEn?: string;
}

export interface DiaryDisplayCell {
    cellType?: DiaryCellTypeEnum;
    value?: any;
}

export interface EventDiaryModel extends EventDiary {
    details?: DiaryCriterionDetail[];
}

export interface EventDiaryDisplayModel {
    empName?: string;
    [key: string]: any;
    days?: number[];
    kpiPoint?: number;
    comment?: string;
}

export interface EmpModel {
    userName?: string;
    userFullName?: string;
    orgName?: string;

    details?: DiaryCriterionDetail[];

    currentKpi?: number;
    currentKpiStatusId?: number;
    moduleName?: string;
    canDeleteEvent?: boolean;
    currentEventDiaryId?: number;
}

export interface EventDiaryConfig {
    id?: number;
    name?: string;
    description?: string;
    orgId?: number;
    orgName?: string;
    level1ManagerUserName?: string;
    level1ManagerFullName?: string;
    level1ManagerJobTitle?: string;
    level1ManagerOrgName?: string;
    level2ManagerUserName?: string;
    level2ManagerFullName?: string;
    level2ManagerJobTitle?: string;
    level2ManagerOrgName?: string;
    createdDate?: Date;
    createdByUser?: string;
    updatedDate?: Date;
    updatedByUser?: string;
    isDeleted?: number;
    code?: string;
    level1ManagerUserId?: number;
    level2ManagerUserId?: number;
    userList?: EVDConfigUserRow[];
    isActive?: boolean;
    orgDirPath?: string;

    // ngày apply date
    applyDate?: Date;
    orgNameEn?: string;
}

export interface EVDConfigUserRow extends UserOrg {
    selectSource?: UserOrg[];
}

export interface Org_Organization {
    id?: number;
    code?: string;
    name?: string;
    description?: string;
    organizationTypeID?: number;
    nodeID?: string;
    directoryPath?: string;
    parentId?: number;
    parentName?: string;
    nameEn?: string;
    isActive?: boolean;
    numberOrder?: number;
}

export interface Org_OrganizationType {
    id?: number;
    code?: string;
    name?: string;
    description?: string;
    isCompany?: boolean;
}

export interface Unlockdiarycriterion {
    STT: number;
    OrganizationId: number;
    YearMonth: number;
    StatusName?: string;
    Organization?: string;
    DirectoryPath?: string;
    Level1ManagerFullName?: string;
    Level2ManagerFullName?: string;
}

export interface Org {
    id?: number;
    name?: string;
    description?: string;
}

export interface Org_JobTitle {
    id?: string;
    title?: string;
    description?: string;
    createdDate?: string;
    createdByUser?: string;
    updatedDate?: string;
    updatedByUser?: string;
    isDeleted?: string;
    deletedDate?: string;
    deletedByUser?: string;
}

export interface View_Statistics_Reports {
    stt: number;
    tongSoNhanVien: number;
    tong: number;
    ap: number;
    apPercent: number;
    a: number;
    aPercent: number;
    am: number;
    amPercent: number;
    bp: number;
    bpPercent: number;
    b: number;
    bPercent: number;
    bm: number;
    bmPercent: number;
    cPercent: number;
    c: number;
    vPercent: number;
    organizationId: number;
    yearMonth: number;
    organization?: string;
    directoryPath?: string;
}

export interface User {
    userName: string;
    name: string;
}

export interface View_KpiEvaluation_Organization {
    colSpan?: number;
    no?: number;
    id?: number;
    userName?: string;
    eventDiaryId?: number;
    employeeName: string;
    employeeJobId: number;
    employeeJobTitle: string;
    level1ManagerUserId: string;
    level1ManagerOrgId: number;
    level1ManagerOrgName: string;
    level1ManagerJobTitleId: number;
    level1ManagerJobTitle: string;
    statusId: number;
    statusName: string;
    kpiMonth: Date;
    kpiMonthNumber: number;
    yearMonth: number;
    empKpiPoint: number;
    empKpiClassification: string;
    level1ManagerKpiPoint: number;
    level1ManagerKpiClassification: string;
    kpiPeriodConfigId: number;
    criterionType: number;
    criterionTypeName: string;
    finalKpiPoint: number;
    finalKpiClassification: string;
    createdDate: Date;
    createdByUser: string;
    approveByLevel1Manager: Date;
    approveByLevel1ManagerId: string;
    approveByLevel2Manager: Date;
    approveByLevel2ManagerId: string;
    organization: string;
    organizationId: number;
    isActived: boolean;
    isDeleted: boolean;
    level1ManagerUserName: string;
    level1ManagerFullName: string;
    level2ManagerUserName: string;
    level2ManagerFullName: string;
    level2ManagerOrgName: string;
    level2ManagerJobTitle: string;
    sentHRDate: Date;
    eventDiaryConfigId: number;
    userId: string;
    hrKpiPoint: number;
    hrKpiPointClassification: string;
    hrKpiPointComment: string;
    finalKpiComment: string;
    submitNote?: string;
    parentId: number;
    directoryPath: string;
    code: string;
    reportPoint: number;
    reportClassification: string;
}

export interface ProcessStatus {
    id?: number;
    title?: string;
    code?: string;
    titleEn?: string;
}

export interface EmpTransfer {
    id?: number;
    code?: string;
    transferDate?: Date;
    userId?: number;
    oldJobTitleId?: number;
    newJobTitleId?: number;
    oldOrgId?: number;
    newOrgId?: number;
    createdBy?: string;
    createdDate?: Date;
    updatedBy?: string;
    updatedDate?: Date;
    oldLevel1MngId?: number;
    newLevel1MngId?: number;
    oldLevel2MngId?: number;
    newLevel2MngId?: number;
    note?: string;
    fromDate?: Date;
    toDate?: Date;

    empFullName?: string;
    oldJobTitleName?: string;
    newJobTitleName?: string;
    oldOrgName?: string;
    newOrgName?: string;
    oldLv1MngName?: string;
    newLv1MngName?: string;
    oldLv2MngName?: string;
    newLv2MngName?: string;
}

export interface UserEmployment {
    fromDate?: Date;
    toDate?: Date;
    orgName?: string;
    jobTitle?: string;
}

export interface DivMngPerUser {
    userId?: number;
    userName?: string;
    userFullName?: string;
    orgName?: string;
    jobTitle?: string;
    details?: DivMngPer[];
}

export interface DivMngPer {
    userId?: number;
    orgId?: number;
    orgName?: string;
}

export interface Kpi_CriterionType {
    id?: number;
    code?: string;
    name?: string;
    catalogs?: Kpi_CriterionTypeCatalog[];
}

export interface Kpi_CriterionTypeCatalog {
    id?: number;
    criterionTypeId?: number;
    criterionCatalogId?: number;
    catalogName?: string;
    startPoint?: number;
}

export interface KpiNotification {
    id?: number;
    status?: number;
    notes?: string;
    action?: string;
    fromUserName?: string;
    toUserName?: string;
    userDate?: Date;
    type?: number;
    nameType?: string;
    modules?: string;
    notesEn?: string;
}

export interface KpiNotiNotificationType {
    id?: number;
    type?: number;
    typeName?: string;
    actionName?: string;
    objectName?: string;
}

// export interface FileData {
//     id?: number;
//     name?: string;
//     url?: string;
// }

export interface FileModel {
    id?: number;
    fileName?: string;
    fileType?: string;
    fileSize?: number;
    isRecentlyCreated?: boolean;
    isRecentlyDeleted?: boolean;
}

export interface E_VoucherType {
    id?: number;
    code?: string;
    name?: string;
    denominations?: number;
    oderNumber?: number;
    isValidate?: boolean;
    description?: string;
    createdDate?: Date;
    createUser?: string;
    updateDate?: Date;
    updateUser?: string;
}

export interface E_VoucherBudget {
    id?: number;
    companyId?: number;
    companyName?: string;
    status?: number;
    statusName?: string;
    totalStaff?: number;
    staffConfirm?: number;
    budget?: number;
    totalValues?: number;
    budgetDate?: Date;
    budgetMonth?: number;
    budgetYear?: number;
    createDate?: Date;
    createUser?: string;
    updateDate?: Date;
    updateUser?: string;
    budgetDetails?: E_VoucherBudgetDetail[];
    budgetDenominations?: E_VoucherBudgetDenominations[];
    denomiTotalValues?: number;
    denomiTotalCount?: number;
    totalDistributedCount?: number;
}
export interface E_VoucherBudgetDetail {
    id?: number;
    voucherBudgetId?: number;
    codeUser?: string;
    nameUser?: string;
    jobTitle?: string;
    orgName?: string;
    orgId?: number;
    orgCompanyName?: string;
    orgCompanyId?: number;
    status?: number;
    statusName?: string;
    budget?: number;
    createdDate?: Date;
    createdUser?: string;
    isDeleted?: number;
    updateDate?: Date;
    updateUser?: string;
    voucheTypeName?: string;
    voucheTypeCode?: string;
    denominations?: number;
    countNumberPage?: number;
    totalValues?: number;
    orderNo?: number;
    distributeLines?: E_VoucherBudgetDetailLine[];
}
export interface E_VoucherBudgetDenominations {
    voucherBudgetId?: number;
    voucherTypeName?: string;
    voucherTypeCode?: string;
    denominations?: number;
    countNumber?: number;
    totalValues?: number;
}
export interface E_VoucherBudgetDetailLine {
    id?: number;
    voucherTypeName?: string;
    voucherTypeCode?: string;
    denominations?: number;
    countNumberPage?: number;
    totalValues?: number;
    voucherBudgetDetailId?: number;
}
export interface ImportDataErrorRow {
    message?: string;
    data?: E_VoucherBudgetDetail[];
}