import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import configurl from '../../assets/config/config.json'
import { EstadoCivil } from '../Models/EstadoCivil';

@Injectable({
  providedIn: 'root'
})
export class EstadoCivilService {

  urlEstadoCivil = configurl.apiServer.url + '/api/EstadoCivil/';
  constructor(private http: HttpClient) { }
  getEstadoCivilList(): Observable<EstadoCivil[]> {
    return this.http.get<EstadoCivil[]>(this.urlEstadoCivil + 'EstadoCivilsList');
  }
}
