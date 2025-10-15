import { AfterViewInit, Component, ElementRef, EventEmitter, input, Input, OnInit, Output, SimpleChanges, viewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { VoluntarioService } from '../../services/voluntario.service';
import { CadastrarVoluntario } from '../../models/cadastrar-voluntario';

@Component({
  selector: 'app-cadastrar-voluntario',
  standalone: false,
  templateUrl: './cadastrar-voluntario.component.html',
})
export class CadastrarVoluntarioComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  voluntarioForm!: FormGroup;
  verSenha = false;

  arquivoSelecionado!: File;
  certificadoPath!: string;
  isArquivoArrastado = false;

  voluntario!: CadastrarVoluntario;
  errors: any[] = [];

  @Input() temVoluntario!: boolean;
  @Output() cadastroConcluido = new EventEmitter<void>();


  constructor(private formBuilder: FormBuilder,
    private voluntarioService: VoluntarioService,
    private toastr: ToastrService) {
    super();

    this.validationMessages = {
      certificado: { required: 'Favor selecionar o certificado.' },
      senha: { required: 'Favor preencher a senha.' }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.voluntarioForm);
  }

  ngOnInit(): void {
    this.voluntarioForm = this.formBuilder.group({
      certificado: ['', Validators.required],
      senha: ['', Validators.required],
      upload: [''],
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

    this.voluntarioForm.patchValue({
      certificado: certificadoName,
      upload: uploadBase64,
    });
    this.voluntarioForm.get('certificado')?.markAsTouched();
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
    if (this.voluntarioForm.dirty && this.voluntarioForm.value) {

      this.voluntario = Object.assign({}, this.voluntario, this.voluntarioForm.value);

      this.voluntarioService.cadastrar(this.voluntario)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        });
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
