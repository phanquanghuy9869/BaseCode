import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import * as _moment from 'moment';
import { Moment } from 'moment';
import { FormControl } from '@angular/forms';
import { MatDatepicker, DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material';
import { DatePipe } from '@angular/common';
import { MomentDateAdapter } from '@angular/material-moment-adapter';

const moment = _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};
@Component({
  selector: 'app-month-picker',
  templateUrl: './month-picker.component.html',
  styleUrls: ['./month-picker.component.css'], providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class MonthPickerComponent implements OnInit {
  dateCtrl = new FormControl(moment());
  controlLabel = "'lblBanKHDGCV' | translate";
  @Input() inputYm: number;
  @Output() changedTrigger = new EventEmitter();

  @Input()
  controlText: string;

  constructor(private _datepipe: DatePipe) { }

  ngOnInit() {
    if (this.controlText) {
      this.controlLabel = this.controlText;
    }
    this.initDateValue();
  }

  initDateValue() {
    if (this.inputYm == null) {
      return;
    }
    this.setDateValue(this.inputYm);
  }

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.dateCtrl.value;
    ctrlValue.year(normalizedYear.year());
    this.dateCtrl.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.dateCtrl.value;
    ctrlValue.month(normalizedMonth.month());
    this.dateCtrl.setValue(ctrlValue);
    this.onChange();
    datepicker.close();
  }

  getDateValue(ym: number) {
    const year = (ym + '').substr(0, 4);
    const month = (ym + '').substr(4);

    const rs = moment('01/' + month + '/' + year, 'DD/MM/YYYY');
    return rs;
  }

  public setDateValue(yearMonth: number) {
    var dt = this.getDateValue(yearMonth);
    this.dateCtrl.setValue(dt);
  }

  public setDate(yearMonthDate: Date) {
    this.dateCtrl.setValue(yearMonthDate);
  }

  getYearMonth(): number {
    if (this.dateCtrl.value == null) {
      return null;
    }

    const yearMonth = this._datepipe.transform(this.dateCtrl.value, 'yyyyMM');
    return +yearMonth;
  }
  
  onChange() {
    // this.changedTrigger.emit(this.getYearMonth());
  }
}
