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
  constructor(private http: HttpClient) { }

  getAllVacancies(): Observable<Vacancie[]>{
    return this.http.get<Vacancie[]>(this.baseUrl + '/Vacancie/GetAllVacancie');
  }
  createVacancies(vacancie: Vacancie): Observable<Vacancie>{
    return this.http.post<Vacancie>(this.baseUrl + '/Company/CreateVacancy', vacancie);
  }
  editVacancies(id: number, vacancie: Vacancie): Observable<Vacancie>{
    return this.http.put<Vacancie>(this.baseUrl + '/Vacancie/EditVacancie/'+ id, vacancie);
  }
  getVacancies(id: string): Observable<Vacancie>{
    return this.http.get<Vacancie>(this.baseUrl + '/Vacancie/GetOneVacancie/' + id);
  }
  deleteVacancies(id: number): Observable<Vacancie>{
    return this.http.delete<Vacancie>(this.baseUrl + '/Vacancie/DeleteVacancie/' + id);
  }
}
