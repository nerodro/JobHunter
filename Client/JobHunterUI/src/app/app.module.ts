import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { VacanciesListComponent } from './Components/Vacancies/vacancies-list/vacancies-list.component';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { RegistrationComponent } from './Components/AuthenticationUser/registration/registration.component';
import { LoginComponent } from './Components/AuthenticationUser/login/login.component';
import { FormsModule } from '@angular/forms';
import { AuthInterceptor } from './Services/AuthenticationUser/auth.interceptor';
import { CorsRequestInterceptor } from './Services/Cors/cors.interceptor';
import { VacanciesAddComponent } from './Components/Vacancies/vacancies-add/vacancies-add.component';
import { VacanciesEditComponent } from './Components/Vacancies/vacancies-edit/vacancies-edit.component';
import { VacanciesViewComponent } from './Components/Vacancies/vacancies-view/vacancies-view.component';

@NgModule({
  declarations: [
    AppComponent,
    VacanciesListComponent,
    RegistrationComponent,
    LoginComponent,
    VacanciesAddComponent,
    VacanciesEditComponent,
    VacanciesViewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },
  /*{
    provide: HTTP_INTERCEPTORS,
    useClass: CorsRequestInterceptor,
    multi: true
  }*/],
  bootstrap: [AppComponent]
})
export class AppModule { }
