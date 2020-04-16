import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserImportSuccessComponent } from './user-import-success.component';

describe('UserImportSuccessComponent', () => {
  let component: UserImportSuccessComponent;
  let fixture: ComponentFixture<UserImportSuccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserImportSuccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserImportSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
