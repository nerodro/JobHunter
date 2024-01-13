import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { VacanciesService } from 'src/app/Services/Vacancies/vacancies.service';
import { Vacancie } from 'src/app/models/vacancie.model';

@Component({
  selector: 'app-vacancies-add',
  templateUrl: './vacancies-add.component.html',
  styleUrls: ['./vacancies-add.component.css']
})
export class VacanciesAddComponent {
  vacancie = new Vacancie();
  constructor(private vacancieService: VacanciesService,private router: Router) {}
  addVacancie(vacancie: Vacancie): void{
    this.vacancieService.createVacancies(vacancie).subscribe({
      next: res =>{
        this.router.navigate(['vacancies'])
      },
      error: (response) => {
        console.log(vacancie);
        console.log(response);
      }
    });
  }
}
