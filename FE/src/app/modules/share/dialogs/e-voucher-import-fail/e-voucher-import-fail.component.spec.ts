import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EVoucherImportFailComponent } from './e-voucher-import-fail.component';

describe('EVoucherImportFailComponent', () => {
  let component: EVoucherImportFailComponent;
  let fixture: ComponentFixture<EVoucherImportFailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EVoucherImportFailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EVoucherImportFailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
