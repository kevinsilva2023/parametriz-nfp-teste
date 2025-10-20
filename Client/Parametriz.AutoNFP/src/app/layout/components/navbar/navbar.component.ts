import { Component, Input, OnInit } from '@angular/core';
import { Usuario } from 'src/app/configuracoes/usuarios/models/usuario';
import { UsuarioService } from 'src/app/configuracoes/usuarios/services/usuario.service';
import { PerfilService } from 'src/app/services/perfil.service';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  @Input() tituloPagina = 'Configurações';
  usuario!: any;
  fotoUpload!: any;

  constructor(private perfilService: PerfilService) {}

  ngOnInit(): void {
    this.preencherNomeUsuarioAtivo();
  }

  preencherNomeUsuarioAtivo() {
    let usuarioLocal = LocalStorageUtils.obterUsuario();
    this.usuario = usuarioLocal;


    this.perfilService.obter()
      .subscribe({
        next: (response: Usuario) => this.fotoUpload = response.fotoUpload ,
        error: (err) => console.log(err)
      })
  }
}
