import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EVoucherImportSuccessComponent } from './e-voucher-import-success.component';

describe('EVoucherImportSuccessComponent', () => {
  let component: EVoucherImportSuccessComponent;
  let fixture: ComponentFixture<EVoucherImportSuccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EVoucherImportSuccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EVoucherImportSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
