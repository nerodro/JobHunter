import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VacanciesEditComponent } from './vacancies-edit.component';

describe('VacanciesEditComponent', () => {
  let component: VacanciesEditComponent;
  let fixture: ComponentFixture<VacanciesEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VacanciesEditComponent]
    });
    fixture = TestBed.createComponent(VacanciesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
