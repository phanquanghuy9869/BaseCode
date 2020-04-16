import { LayoutModule } from '@angular/cdk/layout';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MatButtonModule, MatDatepickerModule, MatDialogModule, MatFormFieldModule, MatIconModule, MatInputModule, MatListModule, MatNativeDateModule, MatPaginatorModule, MatProgressSpinnerModule, MatSelectModule, MatSidenavModule, MatSortModule, MatTableModule, MatTabsModule, MatToolbarModule, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatCheckboxModule, MatTooltipModule, MatBadgeModule } from '@angular/material';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { AlertDialogComponent } from './dialogs/alert-dialog/alert-dialog.component';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { AddDiaryDialogComponent } from './dialogs/add-diary-dialog/add-diary-dialog.component';
import { UserDiaryDetailsComponent } from './dialogs/user-diary/user-diary-details/user-diary-details.component';
import { MonthPickerComponent } from './components/month-picker/month-picker.component';
import { ChangePasswordDialogComponent } from './dialogs/change-password-dialog/change-password-dialog.component';
import { UpdateKpiDialogComponent } from './dialogs/update-kpi-dialog/update-kpi-dialog.component';
import { PipeModule } from 'src/app/pipe/pipe.module';
import { NotificationDialogComponent } from './dialogs/notification-dialog/notification-dialog.component';
import { NgxMatSelectSearchModule } from './components/ngx-mat-select/public_api';
import { FileUploaderComponent } from './components/file-uploader/file-uploader.component';
import { NgxExcelImportComponent } from './components/ngx-excel-import/ngx-excel-import.component';
import { EVoucherImportSuccessComponent } from './dialogs/e-voucher-import-success/e-voucher-import-success.component';
import { EVoucherImportFailComponent } from './dialogs/e-voucher-import-fail/e-voucher-import-fail.component';
import { UserImportSuccessComponent } from './dialogs/user-import-success/user-import-success.component';
import { UserImportFailComponent } from './dialogs/user-import-fail/user-import-fail.component';
import { QrCodeComponent } from './components/qr-code/qr-code.component';
import { BarCodeComponent } from './components/bar-code/bar-code.component';
import { QRCodeModule } from 'angularx-qrcode';
import { NgxBarcodeModule } from 'ngx-barcode';


export const MY_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MM YYYY',
  },
};

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    // ReactiveFormsModule,
    HttpClientModule,
    AngularFontAwesomeModule,
    // LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatFormFieldModule,
    RouterModule,
    FlexLayoutModule,
    MatDialogModule,
    MatTooltipModule,
    PipeModule,
    MatBadgeModule,
    NgxMatSelectSearchModule,
    QRCodeModule,
    NgxBarcodeModule
  ],
  declarations: [SideNavComponent, AlertDialogComponent,
    ConfirmDialogComponent, AddDiaryDialogComponent, UserDiaryDetailsComponent, MonthPickerComponent, ChangePasswordDialogComponent
    , UpdateKpiDialogComponent, NotificationDialogComponent, FileUploaderComponent, NgxExcelImportComponent, EVoucherImportSuccessComponent, EVoucherImportFailComponent
    , UserImportSuccessComponent, UserImportFailComponent, QrCodeComponent, BarCodeComponent],
  entryComponents: [AlertDialogComponent, ConfirmDialogComponent, AddDiaryDialogComponent, UserDiaryDetailsComponent
    , ChangePasswordDialogComponent, UpdateKpiDialogComponent, NotificationDialogComponent,EVoucherImportSuccessComponent, EVoucherImportFailComponent
   , UserImportSuccessComponent, UserImportFailComponent],
  exports: [
    CommonModule,
    FormsModule,
    // ReactiveFormsModule,
    HttpClientModule,
    AngularFontAwesomeModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatInputModule,
    MatTabsModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatFormFieldModule,
    FlexLayoutModule,
    MatDialogModule,
    MonthPickerComponent,
    MatCheckboxModule,
    FileUploaderComponent,
    NgxExcelImportComponent,
    QrCodeComponent,
    BarCodeComponent,
    QRCodeModule,
    NgxBarcodeModule
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
    DatePipe
  ]
})
export class ShareModule { }
