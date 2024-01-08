import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VacanciesListComponent } from './Components/Vacancies/vacancies-list/vacancies-list.component';

const routes: Routes = [
  {
    path: '',
    component: VacanciesListComponent
  },
  {
    path: 'vacancies',
    component: VacanciesListComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
