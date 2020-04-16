import { Component, OnInit, Input } from '@angular/core';
import { EVoucherViewModel } from 'src/app/models/data/evoucher';

@Component({
  selector: 'app-e-voucher-detail',
  templateUrl: './e-voucher-detail.component.html',
  styleUrls: ['./e-voucher-detail.component.css']
})
export class EVoucherDetailComponent implements OnInit {
  @Input() model: EVoucherViewModel = {emp: '0001009999', price: 2000000, data: 'this is a test', startDate: new Date(), endDate: new Date() }; 

  constructor() { }

  ngOnInit() {
  }

}
