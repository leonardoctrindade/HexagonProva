import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import configurl from '../../assets/config/config.json'
import { Cidade } from '../Models/Cidade';

@Injectable({
  providedIn: 'root'
})
export class CidadeService {

  urlCidade = configurl.apiServer.url + '/api/Cidade/';
  constructor(private http: HttpClient) { }

  getCidadesListByEstado(estadoId: string): Observable<Cidade[]> {
    return this.http.get<Cidade[]>(this.urlCidade + 'CidadesListByEstado?estadoId=' + estadoId);
  }

}
