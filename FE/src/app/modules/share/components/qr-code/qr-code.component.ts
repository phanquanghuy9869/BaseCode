import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ngx-qr-code',
  templateUrl: './qr-code.component.html',
  styleUrls: ['./qr-code.component.css']
})
export class QrCodeComponent implements OnInit {
  @Input() data = 'default';    

  constructor() { }

  ngOnInit() {
  }

}
