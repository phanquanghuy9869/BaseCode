import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShareModule } from 'src/app/modules/share/share.module';
import { Routes, RouterModule } from '@angular/router';
import { EVoucherCodeImportComponent } from 'src/app/modules/e-voucher-import/e-voucher-code-import/e-voucher-code-import.component';
import { NgxMatSelectSearchModule } from 'src/app/modules/share/components/ngx-mat-select/ngx-mat-select-search.module';

export const routes: Routes = [
  { path: 'import-voucher-code', component: EVoucherCodeImportComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    ShareModule,
    NgxMatSelectSearchModule
  ],
  declarations: [EVoucherCodeImportComponent]
})
export class EVoucherImportModule { }
