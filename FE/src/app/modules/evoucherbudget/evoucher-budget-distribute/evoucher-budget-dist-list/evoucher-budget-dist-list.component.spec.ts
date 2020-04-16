import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherBudgetDistListComponent } from './evoucher-budget-dist-list.component';

describe('EvoucherBudgetDistListComponent', () => {
  let component: EvoucherBudgetDistListComponent;
  let fixture: ComponentFixture<EvoucherBudgetDistListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherBudgetDistListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherBudgetDistListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
