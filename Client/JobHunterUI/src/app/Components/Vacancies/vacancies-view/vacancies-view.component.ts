import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VacanciesService } from 'src/app/Services/Vacancies/vacancies.service';
import { Vacancie } from 'src/app/models/vacancie.model';

@Component({
  selector: 'app-vacancies-view',
  templateUrl: './vacancies-view.component.html',
  styleUrls: ['./vacancies-view.component.css']
})
export class VacanciesViewComponent {
  vacancie: Vacancie = new Vacancie();
  constructor(private route: ActivatedRoute, public vacancieService: VacanciesService){}
  ngOnInit(): void{
    this.route.paramMap.subscribe({
      next:(params) => {
        const id = params.get('id')
        if(id){
          this.vacancieService.getVacancies(id).subscribe({
            next: (response) => {
              this.vacancie = response;
            }
          });
        }
      }
    })
  }
}
