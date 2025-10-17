import { Component, OnInit } from '@angular/core';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ActivatedRoute, Data } from '@angular/router';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { CupomFiscal, CupomFiscalResponse } from '../../models/cupom-fiscal';

@Component({
  selector: 'app-listar-cupom-fiscal',
  standalone: false,
  templateUrl: './listar-cupom-fiscal.component.html',
  styles: `
    .sticky-top { z-index: 999 !important; }

    ::ng-deep .mat-mdc-form-field-subscript-wrapper {
      display: none !important;
    }

  .card-equal {
    flex: 1;       
    min-width: 180px; 
  }
  `
})

export class ListarCupomFiscalComponent implements OnInit {

  status!: Enumerador[];
  usuariosAtivos!: ObterUsuarioAtivo[];

  cuponsFiscaisResponse: CupomFiscalResponse = new CupomFiscalResponse();
  totalProcessadas!: number;
  percentualSucesso!: number;

  // filtro
  data = new Date();
  filtroCompetencia = this.data;
  filtroUsuario = '';
  filtroStatus = '';

  // paginator
  pagina = 1;
  filtroRegistroPorPagina = 5;
  totalItems = 0;

  constructor(private activatedRoute: ActivatedRoute,
    private cupomFiscalService: CupomFiscalService) {
    this.status = this.activatedRoute.snapshot.data['status'];
    this.usuariosAtivos = this.activatedRoute.snapshot.data['usuariosAtivos'];
  }

  ngOnInit(): void {
    this.obterPorFiltro();
  }

  obterPorFiltro() {
    this.cupomFiscalService
      .obterPorFiltro(this.filtroCompetencia, this.filtroUsuario, this.filtroStatus, this.pagina, this.filtroRegistroPorPagina)
      .subscribe({
        next: (response: any) => {
          this.cuponsFiscaisResponse = response;
          this.obterTotalProcessadas();
          this.obterPercentualSucesso();
        },
        error: (err) => console.log(err)
      })
  }

  onPageChange(page: number) {
    this.pagina = page;
    this.obterPorFiltro();
  }

  obterTotalProcessadas() {
    this.totalProcessadas = this.cuponsFiscaisResponse.erro + this.cuponsFiscaisResponse.sucesso;
    console.log(this.totalProcessadas);
  }

  obterPercentualSucesso() {
    this.totalProcessadas > 0
      ? this.percentualSucesso = Number(((this.cuponsFiscaisResponse.sucesso / this.totalProcessadas) * 100).toFixed(2))
      : 0;
  }

}
