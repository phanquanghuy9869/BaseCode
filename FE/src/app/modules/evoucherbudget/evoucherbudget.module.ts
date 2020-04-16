import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShareModule } from '../share/share.module';
import { AuthGuard } from '../auth.guard';
import { NgxMatSelectSearchModule } from 'src/app/modules/share/components/ngx-mat-select/ngx-mat-select-search.module';
import { PipeModule } from 'src/app/pipe/pipe.module';
import { EvoucherTypeListComponent } from './evoucher-type/evoucher-type-list/evoucher-type-list.component';
import { EvoucherTypeAddOrEditComponent } from './evoucher-type/evoucher-type-add-or-edit/evoucher-type-add-or-edit.component';
import { EvoucherBudgetListComponent } from './evoucher-budget/evoucher-budget-list/evoucher-budget-list.component';
import { EvoucherBudgetInputDialogComponent } from './evoucher-budget/evoucher-budget-input-dialog/evoucher-budget-input.component';
import { EvoucherBudgetInputSuccessDialogComponent } from './evoucher-budget/evoucher-budget-input-success-dialog/evoucher-budget-input-success-dialog.component';
import { EvoucherBudgetInputErrorDialogComponent } from './evoucher-budget/evoucher-budget-input-error-dialog/evoucher-budget-input-error-dialog.component';
import { EvoucherBudgetInfoComponent } from './evoucher-budget/evoucher-budget-info/evoucher-budget-info.component';
import { EvoucherBudgetDistListComponent } from './evoucher-budget-distribute/evoucher-budget-dist-list/evoucher-budget-dist-list.component';
import { EvoucherBudgetDistDetailComponent } from './evoucher-budget-distribute/evoucher-budget-dist-detail/evoucher-budget-dist-detail.component';
import { EvoucherBudgetDistLineDialogComponent } from './evoucher-budget-distribute/evoucher-budget-dist-detail-line/evoucher-budget-dist-detail-line.component';

export const routes: Routes = [
  { path: 'ev-type-list/:page', component: EvoucherTypeListComponent, canActivate: [AuthGuard] },
  { path: 'ev-type-detail/:id/:page', component: EvoucherTypeAddOrEditComponent },
  { path: 'ev-budget', component: EvoucherBudgetListComponent },
  { path: 'ev-budget-info/:id', component: EvoucherBudgetInfoComponent },
  { path: 'ev-budget-input', component: EvoucherBudgetInputDialogComponent },
  { path: 'ev-budget-dist', component: EvoucherBudgetDistListComponent },
  { path: 'ev-budget-dist/:id', component: EvoucherBudgetDistDetailComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    ShareModule,
    NgxMatSelectSearchModule,
    PipeModule
  ],
  declarations: [
    EvoucherTypeListComponent,
    EvoucherTypeAddOrEditComponent,
    EvoucherBudgetListComponent,
    EvoucherBudgetInfoComponent,
    EvoucherBudgetInputDialogComponent,
    EvoucherBudgetInputSuccessDialogComponent,
    EvoucherBudgetInputErrorDialogComponent,
    EvoucherBudgetDistListComponent,
    EvoucherBudgetDistDetailComponent,
    EvoucherBudgetDistLineDialogComponent],
  entryComponents: [EvoucherBudgetInputDialogComponent, EvoucherBudgetInputSuccessDialogComponent, EvoucherBudgetInputErrorDialogComponent
    , EvoucherBudgetDistLineDialogComponent],
})
export class EvoucherBudgetModule { }
