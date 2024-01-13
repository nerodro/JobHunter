import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { City } from 'src/app/models/city.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CityService {

  baseUrl: string = environment.baseUrl;
  constructor(private http: HttpClient) { }

  getAllCity(): Observable<City[]>{
    return this.http.get<City[]>(this.baseUrl + '/City/GetAllCity');
  }
}
