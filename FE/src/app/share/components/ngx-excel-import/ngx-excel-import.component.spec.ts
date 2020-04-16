import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NgxExcelImportComponent } from './ngx-excel-import.component';

describe('NgxExcelImportComponent', () => {
  let component: NgxExcelImportComponent;
  let fixture: ComponentFixture<NgxExcelImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NgxExcelImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NgxExcelImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
