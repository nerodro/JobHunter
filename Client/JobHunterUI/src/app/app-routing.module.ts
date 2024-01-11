import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VacanciesListComponent } from './Components/Vacancies/vacancies-list/vacancies-list.component';
import { LoginComponent } from './Components/AuthenticationUser/login/login.component';
import { RegistrationComponent } from './Components/AuthenticationUser/registration/registration.component';

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
    path: 'loginUser',
    component: LoginComponent
  },
  {
    path: 'registerUser',
    component: RegistrationComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
