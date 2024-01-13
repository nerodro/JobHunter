import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VacanciesService } from 'src/app/Services/Vacancies/vacancies.service';
import { Vacancie } from 'src/app/models/vacancie.model';

@Component({
  selector: 'app-vacancies-edit',
  templateUrl: './vacancies-edit.component.html',
  styleUrls: ['./vacancies-edit.component.css']
})
export class VacanciesEditComponent {
  vacancie: Vacancie = new Vacancie();
  constructor(private route: ActivatedRoute, public vacancieService: VacanciesService,private router: Router){}
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
  EditVacancie(vacancie: Vacancie):void{
    this.vacancieService.editVacancies(vacancie.id, vacancie).subscribe({
      next: res =>{
        this.router.navigate(['vacancies'])
      },
      error: (response) => {
        console.log(vacancie);
        console.log(response);
      }
    });
  }
  deleteVacancie(id: number):void{
    this.vacancieService.deleteVacancies(id).subscribe({
      next: res =>{
        this.router.navigate(['vacancies'])
      },
      error: (response) => {
        this.router.navigate(['vacancies'])
        console.log(response);
      }
    });
  }
}
