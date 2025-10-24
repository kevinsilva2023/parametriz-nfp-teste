import { Component, Input, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
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

  voluntarioNome!: string;
  fotoUpload!: any;
  claimAdmin!: string;

  razaoSocialInstituicao!: string;
  cnpj!: string;
  certificadoStatusNome!: any;
  certificadoStatus!: any;

  constructor(
    private perfilService: PerfilService,
    private autorizacaoService: AutorizacaoService
  ) { }

  ngOnInit(): void {
    this.preencherVoluntarioAtivo();
    this.obterInsituicao();

    PerfilService.atualizarNavSubject
      .subscribe({
        next: () => {
          this.preencherVoluntarioAtivo();
        }
      });
  }

  preencherVoluntarioAtivo() {
    let claimAdmin: Claim = { type: 'role', value: 'Administrador' };
    let voluntarioEhAdmin = this.autorizacaoService.voluntarioPossuiClaim(claimAdmin);

    this.claimAdmin = voluntarioEhAdmin ? 'Administrador' : 'Usuário';

    this.perfilService.obter()
    .subscribe({
      next: (voluntario: Voluntario) => (
        this.fotoUpload = voluntario.fotoUpload,
        this.voluntarioNome = voluntario.nome,
        this.certificadoStatusNome = voluntario.certificadoStatusNome,
        this.certificadoStatus = voluntario.certificadoStatus
      ),
    });

  }

  obterInsituicao() {
    var result = LocalStorageUtils.obterUsuario();

    this.razaoSocialInstituicao = result.instituicao.razaoSocial;
    this.cnpj = result.instituicao.cnpj;
  }

}
