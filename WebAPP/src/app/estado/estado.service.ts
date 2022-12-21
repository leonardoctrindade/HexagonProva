import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import configurl from '../../assets/config/config.json'
import { Estado } from '../Models/Estado';

@Injectable({
  providedIn: 'root'
})
export class EstadoService {

  urlEstado = configurl.apiServer.url + '/api/Estado/';
  constructor(private http: HttpClient) { }
  getEstadoList(): Observable<Estado[]> {
    return this.http.get<Estado[]>(this.urlEstado + 'EstadosList');
  }
}
