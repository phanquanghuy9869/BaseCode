import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetInputDialogComponent } from './evoucher-budget-input.component';

describe('EvoucherBudgetInputComponent', () => {
  let component: EvoucherBudgetInputDialogComponent;
  let fixture: ComponentFixture<EvoucherBudgetInputDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetInputDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetInputDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
