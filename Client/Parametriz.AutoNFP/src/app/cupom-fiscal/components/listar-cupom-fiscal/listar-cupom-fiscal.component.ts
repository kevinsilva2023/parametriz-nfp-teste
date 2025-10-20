import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ActivatedRoute, Data } from '@angular/router';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { CupomFiscalResponse } from '../../models/cupom-fiscal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VisualizarCupomFiscalComponent } from '../visualizar-cupom-fiscal/visualizar-cupom-fiscal.component';

@Component({
  selector: 'app-listar-cupom-fiscal',
  standalone: false,
  templateUrl: './listar-cupom-fiscal.component.html',
  styles: `
    .sticky-top { z-index: 999 !important; }

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

  data = new Date
  filtroCompetencia = this.data;
  filtroUsuario = '';
  filtroStatus = '';

  pagina = 1;
  filtroRegistroPorPagina = 5;
  totalItems = 0;

  constructor(private activatedRoute: ActivatedRoute,
    private cupomFiscalService: CupomFiscalService,
    private modalService: NgbModal) {
    this.status = this.activatedRoute.snapshot.data['status'];
    this.usuariosAtivos = this.activatedRoute.snapshot.data['usuariosAtivos'];
  }

  ngOnInit(): void {
    this.definirCompetencia();
    this.obterPorFiltro();
  }

  definirCompetencia() {
    const hoje = new Date();
    const mes = hoje.getDate() <= 20 
      ? hoje.getMonth() - 1 
      : hoje.getMonth();

    this.data = new Date(hoje.getFullYear(), mes, 1);
    this.filtroCompetencia = this.data;
  }

  alterarFiltroCadastradoPor(event: any) {
    this.filtroUsuario = event;
    this.obterPorFiltro();
  }

  alterarFiltroStatus(event: any) {
    this.filtroStatus = event;
    this.obterPorFiltro();
  }

  alterarFiltroCompetencia(date: Date) {
    this.filtroCompetencia = date;
    this.obterPorFiltro();
  }

  alterarFiltroPage(page: number) {
    this.pagina = page;
    this.obterPorFiltro();
  }

  alterarFiltroPorPage(event: any) {
    this.filtroRegistroPorPagina = event.value;
    this.pagina = 1;
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

  obterTotalProcessadas() {
    this.totalProcessadas = this.cuponsFiscaisResponse.erro + this.cuponsFiscaisResponse.sucesso;
  }

  obterPercentualSucesso() {
    this.totalProcessadas > 0
      ? this.percentualSucesso = Number(((this.cuponsFiscaisResponse.sucesso / this.totalProcessadas) * 100).toFixed(2))
      : this.percentualSucesso = 0;
  }

  exibirErro(mensagemErro: string) {
    let modalRef = this.modalService.open(VisualizarCupomFiscalComponent, { size: 'md', centered: true });

    modalRef.componentInstance.mensagemErro = mensagemErro;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  get totalPaginas(): number {
    return Math.ceil(this.cuponsFiscaisResponse.total / this.filtroRegistroPorPagina);
  }

}
