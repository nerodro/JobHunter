import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { VacanciesListComponent } from './Components/Vacancies/vacancies-list/vacancies-list.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RegistrationComponent } from './Components/AuthenticationUser/registration/registration.component';
import { LoginComponent } from './Components/AuthenticationUser/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    VacanciesListComponent,
    RegistrationComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
