import { Component, OnInit } from '@angular/core';
import { VacanciesService } from 'src/app/Services/Vacancies/vacancies.service';
import { Vacancie } from 'src/app/models/vacancie.model';

@Component({
  selector: 'app-vacancies-list',
  templateUrl: './vacancies-list.component.html',
  styleUrls: ['./vacancies-list.component.css']
})
export class VacanciesListComponent implements OnInit {
  vacancies: Vacancie[] = [];
  constructor(private vacancieService: VacanciesService){}
  ngOnInit(): void {
    this.vacancieService.getAllVacancies()
    .subscribe({
      next: vacancies => {
        this.vacancies = vacancies;
        console.log(vacancies);
      },
      error: (response) => {
        console.log(response);
      }
    })
  }
  
}
