import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login } from 'src/app/models/login.model';
import { User } from 'src/app/models/user.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationUserService {
  baseUrl: string = environment.baseUrl;
  constructor(private http: HttpClient) { }
  
  public registerUser(user: User): Observable<any>{
    return this.http.post<any>(this.baseUrl + '/Account/Register', user);
  }
  public loginUser(user: Login): Observable<string>{
    return this.http.post(this.baseUrl + '/Account/LoginUser', user, {responseType: 'text'});
  }
}
