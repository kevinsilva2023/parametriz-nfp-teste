import { useAnimation } from '@angular/animations/animation_player.d-Dv9iW4uh';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ColdObservable } from 'rxjs/internal/testing/ColdObservable';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  @Input() tituloPagina: string = 'Página';
  usuario = '';

  ngOnInit(): void {
    this.preencherNomeUsuarioAtivo();
  }

  preencherNomeUsuarioAtivo() {
    let usuario = LocalStorageUtils.obterUsuario();
    this.usuario = usuario.nome;

    console.log(usuario)
  }

}
