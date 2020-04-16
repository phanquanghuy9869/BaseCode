import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetComponent } from './evoucher-budget-list.component';

describe('EvoucherBudgetComponent', () => {
  let component: EvoucherBudgetComponent;
  let fixture: ComponentFixture<EvoucherBudgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
