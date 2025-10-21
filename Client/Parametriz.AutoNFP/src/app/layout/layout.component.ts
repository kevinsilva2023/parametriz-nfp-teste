import { Component, OnInit } from '@angular/core';
import { AutorizacaoService } from '../shared/services/autorizacao.service';
import { Claim } from '../shared/models/claim';
import { Instituicao } from '../identidade/models/instituicao';
import { LocalStorageUtils } from '../shared/utils/local-storage-utils';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit {
  razaoSocialInstituicao!: string;
  cnpj!: string;
  tituloPagina = 'Dashboard';

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute) { }


  ngOnInit(): void {
    this.obterInsituicao();
    this.ouvirMudancaDeRota();
  }

  obterInsituicao() {
    var result = LocalStorageUtils.obterUsuario();
    this.razaoSocialInstituicao = result.instituicao.razaoSocial;
    this.cnpj = result.instituicao.cnpj;
  }

  ouvirMudancaDeRota() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        const rotaAtiva = this.obterRotaMaisProfunda(this.activatedRoute);
        this.tituloPagina = rotaAtiva.snapshot.data['titulo'] || 'Dashboard';
      });
  }

  private obterRotaMaisProfunda(route: ActivatedRoute): ActivatedRoute {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }
}
