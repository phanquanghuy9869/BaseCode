import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetInputSuccessDialogComponent } from './evoucher-budget-input-success-dialog.component';

describe('EvoucherBudgetInputSuccessDialogComponent', () => {
  let component: EvoucherBudgetInputSuccessDialogComponent;
  let fixture: ComponentFixture<EvoucherBudgetInputSuccessDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetInputSuccessDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetInputSuccessDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
