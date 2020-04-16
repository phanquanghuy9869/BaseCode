import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ngx-bar-code',
  templateUrl: './bar-code.component.html',
  styleUrls: ['./bar-code.component.css']
})
export class BarCodeComponent implements OnInit {
  @Input() data: string;

  constructor() { 
  }

  ngOnInit() {   
  }
}
