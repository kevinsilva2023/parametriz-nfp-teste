import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ActivatedRoute, Data } from '@angular/router';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { CupomFiscalPaginacao } from '../../models/cupom-fiscal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VisualizarCupomFiscalComponent } from '../visualizar-cupom-fiscal/visualizar-cupom-fiscal.component';
import { Claim } from 'src/app/shared/models/claim';
import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { ObterVoluntarioAtivo } from 'src/app/shared/models/obter-voluntario-ativo';
import { ErroTransmissaoLote } from 'src/app/erros-transmissao-lote/models/erro-transmissao-lote';
import { ListarErroTransmissaoLoteModalComponent } from 'src/app/erros-transmissao-lote/listar-erro-transmissao-lote-modal/listar-erro-transmissao-lote-modal.component';

@Component({
  selector: 'app-listar-cupom-fiscal',
  standalone: false,
  templateUrl: './listar-cupom-fiscal.component.html',
  styleUrl: './lista-cupom-fiscal.component.scss'
})

export class ListarCupomFiscalComponent implements OnInit {

  status!: Enumerador[];
  usuariosAtivos!: ObterVoluntarioAtivo[];
  errosTransmissaoLote!: ErroTransmissaoLote[];

  cuponsFiscaisResponse: CupomFiscalPaginacao = new CupomFiscalPaginacao();
  totalProcessadas!: number;
  percentualSucesso!: number;

  data = new Date
  filtroCompetencia = this.data;
  filtroUsuario = '';
  filtroStatus = '';

  pagina = 1;
  filtroRegistroPorPagina = 15;
  totalItems = 0;

  claimAdmin: Claim = { type: 'role', value: 'Administrador' };
  usuarioEhAdmin!: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private cupomFiscalService: CupomFiscalService,
    private modalService: NgbModal,
    private autorizacaoService: AutorizacaoService) {
    this.status = this.activatedRoute.snapshot.data['status'];
    this.usuariosAtivos = this.activatedRoute.snapshot.data['usuariosAtivos'];
    this.errosTransmissaoLote = this.activatedRoute.snapshot.data['errosTransmissaoLote']
  }

  ngOnInit(): void {
    this.definirCompetencia();
    this.verificaClaim();
    this.obter();
    this.verificaPossuiErroTransmissaoLote();
  }

  verificaClaim() {
    this.usuarioEhAdmin = this.autorizacaoService.voluntarioPossuiClaim(this.claimAdmin)
  }

  verificaPossuiErroTransmissaoLote() {

    if (this.errosTransmissaoLote.length > 0) {
      let modalRef = this.modalService.open(ListarErroTransmissaoLoteModalComponent, { size: 'lg' })

      modalRef.componentInstance.errosTransmissaoLote = this.errosTransmissaoLote;
    }
  }

  definirCompetencia() {
    const hoje = new Date();
    const mes = hoje.getDate() <= 30 //alterar depois para dia 20
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

  obter() {
    this.usuarioEhAdmin
      ? this.obterPorFiltro()
      : this.obterPorUsuario()
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

  obterPorUsuario() {
    this.cupomFiscalService
      .obterPorVoluntario(this.filtroCompetencia, this.filtroStatus, this.pagina, this.filtroRegistroPorPagina)
      .subscribe({
        next: (response: any) => {
          this.cuponsFiscaisResponse = response;
          this.obterTotalProcessadas();
          this.obterPercentualSucesso();
        }
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
