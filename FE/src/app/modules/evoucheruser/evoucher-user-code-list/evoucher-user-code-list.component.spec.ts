import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EvoucherUserCodeListComponent } from './evoucher-user-code-list.component';

describe('EvoucherUserCodeListComponent', () => {
  let component: EvoucherUserCodeListComponent;
  let fixture: ComponentFixture<EvoucherUserCodeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvoucherUserCodeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvoucherUserCodeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
