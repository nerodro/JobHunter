import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationUserService } from 'src/app/Services/AuthenticationUser/authentication-user.service';
import { CityService } from 'src/app/Services/City/city.service';
import { City } from 'src/app/models/city.model';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  user = new User();
  city: City[] = []
  constructor(private authService: AuthenticationUserService,private router: Router, public cityService: CityService) {}
  register(user: User){
    this.authService.registerUser(user).subscribe({
      next: res =>{
        console.log(user)
        this.router.navigate(['loginUser'])
      },
      error: (response) => {
      }
    });
  }
  ngOnInit(): void {
    this.cityService.getAllCity()
    .subscribe({
      next: city => {
        this.city = city;
        console.log(city);
      },
      error: (response) => {
        console.log(response);
      }
    })
  }
  
}
