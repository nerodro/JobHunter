import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationUserService } from 'src/app/Services/AuthenticationUser/authentication-user.service';
import { Login } from 'src/app/models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginData = new Login();
  constructor(private authService: AuthenticationUserService,private router: Router) {}

  login(loginData: Login){
    this.authService.loginUser(loginData).subscribe((token: string) =>{
      localStorage.setItem('authToken', token),
      this.router.navigate(['vacancies']);
    }, );
  }
}
