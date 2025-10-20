import { Component, OnInit } from '@angular/core';
import { AutorizacaoService } from '../shared/services/autorizacao.service';
import { Claim } from '../shared/models/claim';
import { Instituicao } from '../identidade/models/instituicao';
import { LocalStorageUtils } from '../shared/utils/local-storage-utils';

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent  implements OnInit {
  razaoSocialInstituicao!: string;
  cnpj!: string;

  ngOnInit(): void {
    this.obterInsituicao();
  }

  obterInsituicao() {
    var result = LocalStorageUtils.obterUsuario();
    this.razaoSocialInstituicao = result.instituicao.razaoSocial;
    this.cnpj = result.instituicao.cnpj;
  }
}
