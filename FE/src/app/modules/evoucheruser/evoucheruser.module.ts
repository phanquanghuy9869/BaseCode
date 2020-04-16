import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShareModule } from '../share/share.module';
import { AuthGuard } from '../auth.guard';
import { NgxMatSelectSearchModule } from 'src/app/modules/share/components/ngx-mat-select/ngx-mat-select-search.module';
import { PipeModule } from 'src/app/pipe/pipe.module';
import { EvoucherUserCodeListComponent } from './evoucher-user-code-list/evoucher-user-code-list.component';
import { EvoucherUserUsedComponent } from './evoucher-user-used/evoucher-user-used.component';
import { EvoucherUserExpiryDateComponent } from './evoucher-user-expiry-date/evoucher-user-expiry-date.component';

export const routes: Routes = [
  { path: 'notuser', component: EvoucherUserCodeListComponent, canActivate: [AuthGuard] },
  { path: 'used', component: EvoucherUserUsedComponent, canActivate: [AuthGuard] },
  { path: 'expired', component: EvoucherUserExpiryDateComponent, canActivate: [AuthGuard] },
  { path: 'note', component: EvoucherUserCodeListComponent, canActivate: [AuthGuard] },
 
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    ShareModule,
    NgxMatSelectSearchModule,
    PipeModule
  ],
  declarations: [
    EvoucherUserCodeListComponent,EvoucherUserUsedComponent,EvoucherUserExpiryDateComponent,]
})
export class EvoucherUserModule { }
