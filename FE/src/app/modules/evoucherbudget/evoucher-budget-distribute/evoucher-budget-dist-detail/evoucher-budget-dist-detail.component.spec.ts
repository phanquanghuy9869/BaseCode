import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetDistDetailComponent } from './evoucher-budget-dist-detail.component';

describe('EvoucherBudgetDistDetailComponent', () => {
  let component: EvoucherBudgetDistDetailComponent;
  let fixture: ComponentFixture<EvoucherBudgetDistDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetDistDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetDistDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
