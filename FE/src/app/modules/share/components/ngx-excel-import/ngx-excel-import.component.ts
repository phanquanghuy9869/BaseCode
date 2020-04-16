import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import * as XLSX from 'xlsx';

@Component({
  selector: 'ngx-excel-import',
  templateUrl: './ngx-excel-import.component.html',
  styleUrls: ['./ngx-excel-import.component.css']
})
export class NgxExcelImportComponent implements OnInit {
  @Input() header: any = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
  @Output() onUpload = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }

  addfile(event) {
    const file = event.target.files[0];
    let fileReader = new FileReader();
    fileReader.readAsArrayBuffer(file);
    fileReader.onload = (e) => {
      let arrayBuffer = fileReader.result as ArrayBuffer;
      var data = new Uint8Array(arrayBuffer);
      var arr = new Array();
      for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
      var bstr = arr.join("");
      var workbook = XLSX.read(bstr, { type: "binary" });
      var first_sheet_name = workbook.SheetNames[0];
      console.log('Sheet name: ', first_sheet_name);
      var worksheet = workbook.Sheets[first_sheet_name];
      const rs = XLSX.utils.sheet_to_json(worksheet, { raw: false, header: this.header, defval: null, dateNF: 'dd/MM/yyyy' });
      console.log('Result: ', rs);
      this.onUpload.emit(rs);
    }
  }
}
