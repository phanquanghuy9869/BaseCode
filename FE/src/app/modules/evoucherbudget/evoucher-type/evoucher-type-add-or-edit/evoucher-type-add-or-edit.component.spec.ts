import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvoucherTypeAddOrEditComponent } from './evoucher-type-add-or-edit.component';

describe('EvoucherTypeAddOrEditComponent', () => {
  let component: EvoucherTypeAddOrEditComponent;
  let fixture: ComponentFixture<EvoucherTypeAddOrEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherTypeAddOrEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherTypeAddOrEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
