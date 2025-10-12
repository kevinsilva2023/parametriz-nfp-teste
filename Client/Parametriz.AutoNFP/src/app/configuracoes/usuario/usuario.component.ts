import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, map, startWith, Observable } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CadastrarUsuarioComponent } from './components/cadastrar-usuario/cadastrar-usuario.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';


@Component({
  selector: 'app-usuario',
  standalone: false,
  templateUrl: './usuario.component.html',
})
export class UsuarioComponent implements OnInit {

  isAdministrador = false;

  usuarioAtivoEhAdmin() {
    const usuario = LocalStorageUtils.obterUsuario();

    this.isAdministrador = (usuario?.claims || [])
      .some((claim: { type: string; value: string; }) => claim?.type === 'role' && claim?.value === 'Administrador');

    console.log('É administrador?', this.isAdministrador);

  }

  ngOnInit(): void {
    this.usuarioAtivoEhAdmin();
  }
}
