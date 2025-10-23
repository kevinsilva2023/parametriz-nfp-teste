import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { Certificado } from '../../models/certificado';
import { CertificadoService } from '../../services/certificado.service';
import { CadastrarCertificado } from '../../models/cadastrar-certificado';

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
  certificadoPath = '';
  isArquivoArrastado = false;

  certificado!: CadastrarCertificado;
  errors: any[] = [];

  @Input() temCertificado!: boolean;
  @Output() cadastroConcluido = new EventEmitter<void>();

  constructor(
    private formBuilder: FormBuilder,
    private certificadoService: CertificadoService,
    private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      certificado: { required: 'Favor selecionar o certificado.' },
      senha: { required: 'Favor preencher a senha.' }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastrarCertificadoForm);
  }

  ngOnInit(): void {
    this.cadastrarCertificadoForm = this.formBuilder.group({
      upload: [''],
      senha: ['', Validators.required],
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

    this.preencherForm(uploadBase64);
  }

  preencherForm(uploadBase64: string) {
    this.cadastrarCertificadoForm.patchValue({
      upload: uploadBase64,
    });
  }

  converterParaBase64(arquivo: File): Promise<string> {

    // adicionar verificacao de tamanho de arquivo
    
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

      this.certificadoService.cadastrar(this.certificado)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarFalha(falha); }
        });
    }
  }

  processarSucesso() {
    this.limparErros();
    this.cadastroConcluido.emit();
    this.toastr.success('Voluntario cadastrado com sucesso!', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao cadastrar voluntario.', 'Erro!');
  }

}
