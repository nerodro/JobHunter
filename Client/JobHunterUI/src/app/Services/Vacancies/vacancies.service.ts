import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Vacancie } from 'src/app/models/vacancie.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VacanciesService {

  baseUrl: string = environment.baseUrl;
  list: Vacancie[] = []
  constructor(private http: HttpClient) { }

  getAllVacancies(): Observable<Vacancie[]>{
    return this.http.get<Vacancie[]>(this.baseUrl + '/Vacancie/GetAllVacancie');
  }
}
