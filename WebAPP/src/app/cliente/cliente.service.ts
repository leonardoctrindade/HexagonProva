import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import configurl from '../../assets/config/config.json'
import { Clientes } from '../Models/Clientes';

@Injectable({
  providedIn: 'root'
})
export class ClientesService {

  urlCliente = configurl.apiServer.url + '/api/Cliente/';
  constructor(private http: HttpClient) { }
  getClienteList(): Observable<Clientes[]> {
    return this.http.get<Clientes[]>(this.urlCliente + 'ClienteListEstadoCidade');
  }
  postClientesData(clienteData: Clientes): Observable<Clientes> {
    const httpHeaders = { headers:new HttpHeaders({'Content-Type': 'application/json'}) };
    return this.http.post<Clientes>(this.urlCliente + 'CreateCliente', clienteData, httpHeaders);
  }
  updateClientes(cliente: Clientes): Observable<Clientes> {
    const httpHeaders = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<Clientes>(this.urlCliente + 'UpdateCliente?id=' + cliente.clienteId, cliente, httpHeaders);
  }
  deleteClientesById(id: number): Observable<number> {
    return this.http.post<number>(this.urlCliente + 'DeleteCliente?id=' + id, null);
  }
  getClientesDetailsById(id: string): Observable<Clientes> {
    return this.http.get<Clientes>(this.urlCliente + 'ClienteDetailEstadoCidade?id=' + id);
  }

}
