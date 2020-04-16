import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetInputErrorDialogComponent } from './evoucher-budget-input-error-dialog.component';

describe('EvoucherBudgetInputErrorDialogComponent', () => {
  let component: EvoucherBudgetInputErrorDialogComponent;
  let fixture: ComponentFixture<EvoucherBudgetInputErrorDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetInputErrorDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetInputErrorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
