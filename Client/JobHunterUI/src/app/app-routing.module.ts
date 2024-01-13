import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VacanciesListComponent } from './Components/Vacancies/vacancies-list/vacancies-list.component';
import { LoginComponent } from './Components/AuthenticationUser/login/login.component';
import { RegistrationComponent } from './Components/AuthenticationUser/registration/registration.component';
import { VacanciesAddComponent } from './Components/Vacancies/vacancies-add/vacancies-add.component';
import { VacanciesEditComponent } from './Components/Vacancies/vacancies-edit/vacancies-edit.component';
import { VacanciesViewComponent } from './Components/Vacancies/vacancies-view/vacancies-view.component';

const routes: Routes = [
  {
    path: '',
    component: VacanciesListComponent
  },
  {
    path: 'vacancies',
    component: VacanciesListComponent
  },
  {
    path: 'auth/loginUser',
    component: LoginComponent
  },
  {
    path: 'auth/registerUser',
    component: RegistrationComponent
  },
  {
    path:'vacancies/vacancieAdd',
    component: VacanciesAddComponent
  },
  {
    path:'vacancies/vacancieEdit/:id',
    component: VacanciesEditComponent
  },
  {
    path:'vacancies/vacancieView/:id',
    component: VacanciesViewComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
