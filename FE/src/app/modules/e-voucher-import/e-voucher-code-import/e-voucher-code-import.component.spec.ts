import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EVoucherCodeImportComponent } from './e-voucher-code-import.component';

describe('EVoucherCodeImportComponent', () => {
  let component: EVoucherCodeImportComponent;
  let fixture: ComponentFixture<EVoucherCodeImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EVoucherCodeImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EVoucherCodeImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
