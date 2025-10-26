import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, Subscription } from 'rxjs';
import { Instituicao } from 'src/app/configuracoes/instituicoes/models/instituicao';
import { InstituicaoService } from 'src/app/configuracoes/instituicoes/services/instituicao.service';
import { Voluntario } from 'src/app/configuracoes/voluntarios/models/voluntario';
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
    private instituicaoService: InstituicaoService,
    private autorizacaoService: AutorizacaoService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.preencherVoluntarioAtivo();
    this.preencherInsituicaoAtiva();

    PerfilService.atualizarNavSubject
      .subscribe({
        next: () => this.preencherVoluntarioAtivo()
      });

    InstituicaoService.atualizarNavSubject
      .subscribe({
        next: () => this.preencherInsituicaoAtiva()
      })
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
      error: () => this.toastr.error("Erro ao carregar voluntário", "Erro!")
    });
  }

  preencherInsituicaoAtiva() {
    this.instituicaoService.obter()
      .subscribe({
        next: (instituicao: Instituicao) => {
          this.razaoSocialInstituicao = instituicao.razaoSocial,
          this.cnpj = instituicao.cnpj
        },
        error: () => this.toastr.error("Erro ao carregar insituição", "Erro!")
      });
  }
}
