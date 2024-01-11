import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationUserService } from 'src/app/Services/AuthenticationUser/authentication-user.service';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  user = new User();
  constructor(private authService: AuthenticationUserService,private router: Router) {}
  register(user: User){
    this.authService.registerUser(user).subscribe({
      next: res =>{
        console.log(user)
        this.router.navigate(['loginUser'])
      },
      error: (response) => {
        console.log(user);
        console.log(response);
      }
    });
  }
  
}
