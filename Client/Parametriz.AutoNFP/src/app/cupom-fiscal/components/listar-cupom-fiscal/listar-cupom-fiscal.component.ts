import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ActivatedRoute, Data } from '@angular/router';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { CupomFiscalResponse } from '../../models/cupom-fiscal';
import { debounceTime, Subject } from 'rxjs';
import { CadastrarCupomFiscal } from '../../models/cadastrar-cupom-fiscal';
import { ToastrService } from 'ngx-toastr';

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

  data = new Date();
  filtroCompetencia = this.data;
  filtroUsuario = '';
  filtroStatus = '';

  pagina = 1;
  filtroRegistroPorPagina = 5;
  totalItems = 0;

  debounceQrCodeCupomFiscal = new Subject<string>();

  errors: [] = [];

  @ViewChild('qrInput') qrInput!: ElementRef<HTMLInputElement>;


  constructor(private activatedRoute: ActivatedRoute,
    private cupomFiscalService: CupomFiscalService,
    private toastr: ToastrService) {
    this.status = this.activatedRoute.snapshot.data['status'];
    this.usuariosAtivos = this.activatedRoute.snapshot.data['usuariosAtivos'];
  }

  ngOnInit(): void {
    this.debounceQrCodeCupomFiscal
      .pipe(debounceTime(500))
      .subscribe({
        next: (qrCodeCupomFiscal: string) => {
          this.processarQrCodeCupomFiscal(qrCodeCupomFiscal)
        }
      })
    this.obterPorFiltro();
  }

  processarQrCodeCupomFiscal(qrCodeCupomFiscal: string) {
    let cadastrarCupomFiscal = new CadastrarCupomFiscal();
    cadastrarCupomFiscal.qrCode = qrCodeCupomFiscal;

    this.cupomFiscalService.processar(cadastrarCupomFiscal)
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()
    this.toastr.info('Cupom enviado para processamento.', 'Processando!');
    this.resetarInput();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao enviar cupom para processamento.', 'Erro');
    this.resetarInput();
  }

  resetarInput() {
    this.qrInput.nativeElement.value = '';
    this.qrInput.nativeElement.focus();
  }

  getInputValue(event: Event): string {
    return (event.target as HTMLInputElement).value;
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

  get totalPaginas(): number {
    return Math.ceil(this.cuponsFiscaisResponse.total / this.filtroRegistroPorPagina);
  }

  limparErros() {
    this.errors = [];
  }
}
