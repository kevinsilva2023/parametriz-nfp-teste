import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject } from 'rxjs';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { CadastrarCupomFiscal } from '../../models/cadastrar-cupom-fiscal';
import { InputUtils } from 'src/app/shared/utils/input-utils';

@Component({
  selector: 'app-cadastrar-cupom-fiscal',
  standalone: false,
  templateUrl: './cadastrar-cupom-fiscal.component.html',
  styles: `
      ::ng-deep .mat-mdc-form-field-subscript-wrapper {
      display: none !important;
    }
    `
})
export class CadastrarCupomFiscalComponent implements OnInit {

  @Output() cupomCadastrado = new EventEmitter<void>();

  @ViewChild('qrCodeInput') qrInput!: ElementRef<HTMLInputElement>;
  debounceQrCodeCupomFiscal = new Subject<string>();
  errors: [] = [];

  inputUtils = InputUtils;

  constructor(private cupomFiscalService: CupomFiscalService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.debounceQrCodeCupomFiscal
      .pipe(debounceTime(500))
      .subscribe({
        next: (qrCodeCupomFiscal: string) => {
          this.processarQrCodeCupomFiscal(qrCodeCupomFiscal)
        }
      })
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
    this.cupomCadastrado.emit();
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

  limparErros() {
    this.errors = [];
  }
}
