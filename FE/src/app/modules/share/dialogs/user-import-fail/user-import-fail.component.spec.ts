import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserImportFailComponent } from './user-import-fail.component';

describe('UserImportFailComponent', () => {
  let component: UserImportFailComponent;
  let fixture: ComponentFixture<UserImportFailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserImportFailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserImportFailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
