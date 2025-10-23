import { Component, Input, OnInit } from '@angular/core';
import { Voluntario } from 'src/app/configuracoes/voluntario/models/voluntario';
import { PerfilService } from 'src/app/perfil/services/perfil.service';
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
  @Input() tituloPagina!: string;
  voluntario!: Voluntario;
  fotoUpload!: any;
  claimAdmin!: string;

  razaoSocialInstituicao!: string;
  cnpj!: string;

  constructor(
    private perfilService: PerfilService,
    private autorizacaoService: AutorizacaoService
  ) { }

  ngOnInit(): void {
    this.preencherVoluntarioAtivo();
    this.obterInsituicao();
  }

  preencherVoluntarioAtivo() {
    let voluntarioLocal = LocalStorageUtils.obterUsuario();
    this.voluntario = voluntarioLocal;

    let claimAdmin: Claim = { type: 'role', value: 'Administrador' };
    let voluntarioEhAdmin = this.autorizacaoService.voluntarioPossuiClaim(claimAdmin);

    this.claimAdmin = voluntarioEhAdmin ? 'Administrador' : 'Usuário';

    this.perfilService.obter().subscribe({
      next: (response: Voluntario) => (this.fotoUpload = response.fotoUpload),
    });
  }

  
  obterInsituicao() {
    var result = LocalStorageUtils.obterUsuario();
    this.razaoSocialInstituicao = result.instituicao.razaoSocial;
    this.cnpj = result.instituicao.cnpj;
  }
  

}
