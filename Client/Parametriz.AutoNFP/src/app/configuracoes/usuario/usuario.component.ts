import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, map, startWith, Observable } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CadastrarUsuarioComponent } from './components/cadastrar-usuario/cadastrar-usuario.component';


@Component({
  selector: 'app-usuario',
  standalone: false,
  templateUrl: './usuario.component.html',
})
export class UsuarioComponent implements OnInit {

  constructor(private modalService: NgbModal) {}  
  
  ngOnInit(): void {
    this.cadastrar();
  }

  cadastrar() {
    let modalRef = this.modalService.open(CadastrarUsuarioComponent, { size: 'lg', centered: false});
    let teste = 'kevin'

    modalRef.componentInstance.instituicao = teste
  }

}
