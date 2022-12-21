
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ClientesService } from './cliente.service';
import { EstadoCivilService } from '../estadocivil/estado-civil.service';
import { Clientes } from '../Models/Clientes';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { EstadoCivil } from '../Models/EstadoCivil';
import { Estado } from '../Models/Estado';
import { EstadoService } from '../estado/estado.service';
import { Cidade } from '../Models/Cidade';
import { CidadeService } from '../cidade/cidade.service';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css']
})
export class ClientesComponent implements OnInit {
  actualPage = 1;
  showEdit: boolean = false;
  showAdd: boolean = true;
  submitted = false;
  ClienteList?: Observable<Clientes[]>;
  ClienteList1?: Observable<Clientes[]>;
  EstadoCivilList?: Observable<EstadoCivil[]>;
  EstadoList?: Observable<Estado[]>;
  CidadeList?: Observable<Cidade[]>;
  clienteForm: any;
  massage = "";
  clienteId = 0;
  estadoId: any;
  constructor(private formbulider: FormBuilder,
    private clienteService: ClientesService,
    private router: Router,
    private estadoCivilService: EstadoCivilService,
    private estadoService: EstadoService,
    private cidadeService: CidadeService,
    private jwtHelper: JwtHelperService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.clienteForm = this.formbulider.group({
      Nome: ['', [Validators.required]],
      Idade: ['', [Validators.required]],
      CidadeId: ['', [Validators.required]],
      EstadoCivilId: ['', [Validators.required]],
      Cpf: ['', [Validators.required]],
      EstadoId: ['', [Validators.required]]
    });

  

    this.fillEstado();
    this.fillEstadoCivil();
    this.getClienteList();
  }



  fillEstadoCivil() {
    this.EstadoCivilList = this.estadoCivilService.getEstadoCivilList();
  }

  fillEstado() {
    this.EstadoList = this.estadoService.getEstadoList();
  }

  fillCidadeByEstado(estadoId: string) {
    this.CidadeList = this.cidadeService.getCidadesListByEstado(estadoId);
  }


  getClienteList() {
    this.ClienteList1 = this.clienteService.getClienteList();
    this.ClienteList = this.ClienteList1;
  }

  onReset() {
    this.submitted = false;
    this.clienteForm.reset();
    this.showEdit = false;
    this.showAdd = true;
  }

  PostCliente(cliente: Clientes) {
    this.submitted = true;

    if (this.clienteForm.invalid) {
      return;
    }

    const cliente_Master = this.clienteForm.value;

    this.clienteService.postClientesData(cliente_Master).subscribe(
      () => {
        this.getClienteList();
        this.onReset();
        this.toastr.success('Cliente Salvo com Sucesso!');
      },
      (err) => {
        this.toastr.clear();
        this.toastr.error(err.error);
        this.toastr.clear();
        this.toastr.error(err.error);
      
      }
    );
  }

  get f() { return this.clienteForm.controls; }

  clientesDetailsToEdit(id: string) {
    this.showEdit = true;
    this.showAdd = false;
    this.clienteService.getClientesDetailsById(id).subscribe(clienteResult => {
      this.clienteId = clienteResult.clienteId;
      this.clienteForm.controls['Nome'].setValue(clienteResult.nome);
      this.clienteForm.controls['Idade'].setValue(clienteResult.idade);
      this.clienteForm.controls['EstadoId'].setValue(clienteResult.estadoId);
      this.estadoId = clienteResult.estadoId?.toString();
      this.CidadeList = this.cidadeService.getCidadesListByEstado(this.estadoId);
      this.clienteForm.controls['CidadeId'].setValue(clienteResult.cidadeId);
      this.clienteForm.controls['EstadoCivilId'].setValue(clienteResult.estadoCivilId);
      this.clienteForm.controls['Cpf'].setValue(clienteResult.cpf);
    });
  }
  UpdateClientes(cliente: Clientes) {

    cliente.clienteId = this.clienteId;
    const cliente_Master = this.clienteForm.value;
    this.clienteService.updateClientes(cliente_Master).subscribe(() => {
      this.toastr.success('Cliente Atualizado com Sucesso!');
      this.getClienteList();
      this.onReset();
    }, (err) => {
      this.toastr.clear();
      this.toastr.error(err.error);
      this.toastr.clear();
      this.toastr.error(err.error);
    }
    );
  }

  DeleteClientes(id: number) {
    if (confirm('Deseja Excluir o Cliente?')) {
      this.clienteService.deleteClientesById(id).subscribe(() => {
        this.toastr.success('Cliente Excluído com Sucesso!');
        this.getClienteList();
      });
    }
  }

  Clear(clientes: Clientes) {
    this.clienteForm.reset();
  }

  public logOut = () => {
    localStorage.removeItem("jwt");
    this.router.navigate(["/"]);
  }

  isUserAuthenticated() {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    else {
      return false;
    }
  }

}
