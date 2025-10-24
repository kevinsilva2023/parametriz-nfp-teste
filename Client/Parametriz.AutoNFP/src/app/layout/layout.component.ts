import { Component, OnInit } from '@angular/core';
import { AutorizacaoService } from '../shared/services/autorizacao.service';
import { Claim } from '../shared/models/claim';
// import { Instituicao } from '../identidade/models/cadastrar-instituicao';
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

  tituloPagina!: string;

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.defineRota();
    this.onRotaAlterada();
  }

  onRotaAlterada() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.defineRota();
      });
  }

  defineRota() {
    let rota = this.activatedRoute;

    while (rota.firstChild) {
      rota = rota.firstChild;
    }

    this.tituloPagina = rota.snapshot.data['titulo'];
  }
}
