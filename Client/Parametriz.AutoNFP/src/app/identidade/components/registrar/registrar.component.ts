import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, viewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
// import { Instituicao } from '../../models/cadastrar-instituicao';
import { IdentidadeService } from '../../services/identidade.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { MyCustomValidators } from 'src/app/shared/validators/my-custom-validators';
import { InputUtils } from 'src/app/shared/utils/input-utils';
import { CadastrarInstituicao } from '../../models/cadastrar-instituicao';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrl: './registrar.component.scss',
  standalone: false
})
export class RegistrarComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  registrarForm!: FormGroup;
  errors: any[] = [];
  instituicao!: CadastrarInstituicao;

  inputUtils = InputUtils;

  constructor(
    private formBuilder: FormBuilder,
    private identidadeService: IdentidadeService,
    private router: Router
  ) {
    super();

    this.validationMessages = {
      cnpj: {
        cnpj: 'O CNPJ informado é inválido.'
      },
      razaoSocial: {
        required: 'Razão Social é obrigatório.'
      },
      entidadeNomeNFP: {
        required: 'Nome da Entidade é obrigatório.'
      },
      logradouro: {
        required: 'Logradouro é obrigatório.'
      },
      numero: {
        required: 'Número é obrigatório.'
      },
      bairro: {
        required: 'Bairro é obrigatório.'
      },
      cep: {
        required: 'CEP é obrigatório.'
      },
      municipio: {
        required: 'Município é obrigatório.'
      },
      uf: {
        required: 'UF é obrigatório.'
      },
      voluntarioNome: {
        required: 'Nome do Voluntário é obrigatório.'
      },
      cpf: {
        required: 'CPF é obrigatório.'
      },
      email: {
        required: 'E-mail é obrigatório.'
      },
      contato: {
        required: 'Contato é obrigatório.'
      }
    };

    LocalStorageUtils.limparDadosLocaisUsuario();

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {
    this.registrarForm = this.formBuilder.group({
      cnpj: [null, [Validators.required, MyCustomValidators.cnpj]],
      razaoSocial: [null, Validators.required],
      entidadeNomeNFP: [null, Validators.required],
      endereco: this.formBuilder.group({
        logradouro: [null, Validators.required],
        numero: [null, Validators.required],
        complemento: [null],
        bairro: [null, Validators.required],
        cep: [null, Validators.required],
        municipio: [null, Validators.required],
        uf: [null, Validators.required],
      }),
      voluntarioNome: [null, Validators.required],
      cpf: [null, Validators.required],
      email: [null, Validators.required],
      contato: [null, Validators.required],
    });
  }

  obterCnpj(cnpj: string) {
    this.identidadeService.obterDadosCnpj(cnpj)
      .subscribe({
        next: (response) => this.preencherForm(response),
      })
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.registrarForm);
  }

  preencherForm(dados: any) {
    if (!dados) return;

    this.registrarForm.patchValue({
      razaoSocial: dados.nome,
      endereco: {
        logradouro: dados.logradouro,
        numero: dados.numero,
        complemento: dados.complemento,
        bairro: dados.bairro,
        cep: dados.cep,
        municipio: dados.municipio,
        uf: dados.uf
      },
    });
  }

  efetuarRegistro() {
    super.validarFormulario(this.registrarForm)

    if (this.registrarForm.dirty && this.registrarForm.valid) {

      this.instituicao = Object.assign({}, this.instituicao, this.registrarForm.value);

      console.log(this.instituicao)

      this.identidadeService.registrar(this.instituicao)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarErro(falha); }
        });
    }
  }

  processarSucesso() {
    this.registrarForm.reset();
    this.limparErros();

    // let email = this.instituicao.email;
    // let usuario = this.instituicao.voluntarioNome;

    // this.router.navigate(
    //   ['/confirmar-email-enviado'],
    //   { queryParams: { email, usuario } }
    // );
  }

  processarErro(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
  }

  limparErros() {
    this.errors = [];
  }

}
