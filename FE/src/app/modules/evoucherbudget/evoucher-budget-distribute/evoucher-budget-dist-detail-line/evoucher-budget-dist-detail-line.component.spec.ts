import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetDistDetailLineComponent } from './evoucher-budget-dist-detail-line.component';

describe('EvoucherBudgetDistDetailLineComponent', () => {
  let component: EvoucherBudgetDistDetailLineComponent;
  let fixture: ComponentFixture<EvoucherBudgetDistDetailLineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetDistDetailLineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetDistDetailLineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
