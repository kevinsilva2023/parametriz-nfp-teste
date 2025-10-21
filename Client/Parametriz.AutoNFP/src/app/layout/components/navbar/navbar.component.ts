import { Component, Input, OnInit } from '@angular/core';
import { Usuario } from 'src/app/configuracoes/usuarios/models/usuario';
import { UsuarioService } from 'src/app/configuracoes/usuarios/services/usuario.service';
import { PerfilService } from 'src/app/services/perfil.service';
import { Claim } from 'src/app/shared/models/claim';
import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
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
  claimAdmin!: string;

  constructor(
    private perfilService: PerfilService,
    private autorizacaoService: AutorizacaoService    
  ) { }

  ngOnInit(): void {
    this.preencherUsuarioAtivo();
  }

  preencherUsuarioAtivo() {
    let usuarioLocal = LocalStorageUtils.obterUsuario();
    this.usuario = usuarioLocal;

    let claimAdmin: Claim = { type: 'role', value: 'Administrador' };
    let usuarioEhAdmin = this.autorizacaoService.usuarioPossuiClaim(claimAdmin);

    this.claimAdmin = usuarioEhAdmin ? 'Administrador' : 'Usuário';

    this.perfilService.obter().subscribe({
      next: (response: Usuario) => (this.fotoUpload = response.fotoUpload),
      error: (err) => console.log(err)
    });
  }
}
