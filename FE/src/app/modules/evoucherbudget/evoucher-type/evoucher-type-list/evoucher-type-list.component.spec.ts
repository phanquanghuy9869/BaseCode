import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherTypeListComponent } from './evoucher-type-list.component';

describe('EvoucherTypeListComponent', () => {
  let component: EvoucherTypeListComponent;
  let fixture: ComponentFixture<EvoucherTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
