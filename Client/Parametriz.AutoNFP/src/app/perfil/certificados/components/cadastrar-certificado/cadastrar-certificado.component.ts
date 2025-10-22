import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';

@Component({
  selector: 'app-cadastrar-certificado',
  standalone: false,
  templateUrl: './cadastrar-certificado.component.html',
  styles: ``
})
export class CadastrarCertificadoComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  cadastrarCertificadoForm!: FormGroup;
  verSenha = false;

  arquivoSelecionado!: File;
  certificadoPath!: string;
  isArquivoArrastado = false;

  certificado!: any; //tipo certificado
  errors: any[] = [];

  @Input() temCertificado!: boolean;
  @Output() cadastroConcluido = new EventEmitter<void>();


  constructor(
    private formBuilder: FormBuilder,
    //private voluntarioService: VoluntarioService,
    private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      // montar validacoes
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastrarCertificadoForm);
  }

  ngOnInit(): void {
    this.cadastrarCertificadoForm = this.formBuilder.group({
      // montar formGroup
    });
  }

  upload(evento: any) {
    let arquivo: File;

    if (this.isArquivoArrastado) {
      arquivo = evento.dataTransfer?.files?.[0];
    } else {
      arquivo = evento.target.files?.[0];
    }

    this.configurarArquivo(arquivo);
  }

  async configurarArquivo(arquivo: File) {
    this.certificadoPath = arquivo.name;

    let uploadBase64 = await this.converterParaBase64(arquivo);
    this.preencherForm(this.certificadoPath, uploadBase64);
  }

  preencherForm(certificadoName: string, uploadBase64: string) {

    // alterar metodo para novos atributos

    // this.cadastrarCertificadoForm.patchValue({
    //   certificado: certificadoName,
    //   upload: uploadBase64,
    // });
    // this.cadastrarCertificadoForm.get('certificado')?.markAsTouched();
  }

  converterParaBase64(arquivo: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const leitor = new FileReader();

      leitor.onload = () => {
        const base64 = (leitor.result as string).split(',')[1];
        resolve(base64);
      };

      leitor.onerror = (erro) => reject(erro);

      leitor.readAsDataURL(arquivo);
    });
  }

  limparErros() {
    this.errors = [];
  }

  efetuarCadastroCertificadoVoluntario() {
    if (this.cadastrarCertificadoForm.dirty && this.cadastrarCertificadoForm.value) {

      this.certificado = Object.assign({}, this.certificado, this.cadastrarCertificadoForm.value);

      // this.voluntarioService.cadastrar(this.voluntario)
      //   .subscribe({
      //     next: (sucesso: any) => { this.processarSucesso(sucesso); },
      //     error: (falha: any) => { this.processarFalha(falha); }
      //   });
    }
  }

  processarSucesso(response: any) {
    this.limparErros();
    this.cadastroConcluido.emit();
    this.toastr.success('Voluntario cadastrado com sucesso!', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao cadastrar voluntario.', 'Erro!');
  }

}
