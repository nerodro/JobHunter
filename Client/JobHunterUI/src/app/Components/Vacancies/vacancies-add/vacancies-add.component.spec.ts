import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VacanciesAddComponent } from './vacancies-add.component';

describe('VacanciesAddComponent', () => {
  let component: VacanciesAddComponent;
  let fixture: ComponentFixture<VacanciesAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VacanciesAddComponent]
    });
    fixture = TestBed.createComponent(VacanciesAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
