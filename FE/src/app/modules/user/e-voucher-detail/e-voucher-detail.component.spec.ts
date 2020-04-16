import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EVoucherDetailComponent } from './e-voucher-detail.component';

describe('EVoucherDetailComponent', () => {
  let component: EVoucherDetailComponent;
  let fixture: ComponentFixture<EVoucherDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EVoucherDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EVoucherDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
