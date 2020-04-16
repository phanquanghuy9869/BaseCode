import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserImportComponent } from './user-import/user-import.component';
import { RouterModule, Routes } from '@angular/router';
import { EVoucherCodeImportComponent } from '../e-voucher-import/e-voucher-code-import/e-voucher-code-import.component';
import { ShareModule } from '../share/share.module';
import { NgxMatSelectSearchModule } from '../share/components/ngx-mat-select/public_api';
import { EVoucherDetailComponent } from './e-voucher-detail/e-voucher-detail.component';

export const routes: Routes = [
  { path: 'import-user', component: UserImportComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    ShareModule,
    NgxMatSelectSearchModule
  ],
  declarations: [UserImportComponent, EVoucherDetailComponent]
})
export class UserModule { }
