import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, viewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Instituicao } from '../../models/instituicao';
import { IdentidadeService } from '../../services/identidade.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { supportsPassiveEventListeners } from '@angular/cdk/platform';
import { MyCustomValidators } from 'src/app/shared/validators/my-custom-validators';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrl: './registrar.component.scss',
  standalone: false
})
export class RegistrarComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  registerForm!: FormGroup;
  errors: any[] = [];
  instituicao!: Instituicao
  verSenha = false;
  verSenhaConfirmacao = false;

  requisitosSenha = {
    minuscula: false,
    maiuscula: false,
    numero: false,
    caracterEspecial: false,
    minimo: false
  }

  constructor(private formBuilder: FormBuilder,
              private identidadeService: IdentidadeService,
              private router: Router) 
  {
    super();

    this.validationMessages = {
      razaoSocial: {
        required: 'Favor preencher a razão social.'
      },
      email: {
        required: 'Favor preencher o e-mail.',
        email: 'O e-mail informado não é válido.'
      },
      usuarioNome: {
        required: 'Favor preencher o nome do voluntário.'
      },
      cnpj: {
        required: 'Favor preencher o CNPJ.',
        cnpj: 'O CNPJ informado é inválido.'
      },
      senha: {
        required: 'Favor preencher a senha.',
        minlength: 'A senha deve conter no mínimo 6 caracteres.',
        maxlength: 'A senha deve conter no máximo 50 caracteres.'
      },
      senhaConfirmacao: {
        required: 'Favor preencher a confirmação da senha.',
        equalTo: 'As senhas não coincidem.'
      }
    };

    LocalStorageUtils.limparDadosLocaisUsuario();

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      razaoSocial: [null, Validators.required],
      email: [null, [
        Validators.required,
        Validators.email
      ]],
      usuarioNome: [null, Validators.required],
      cnpj: [null, [
        Validators.required,
        MyCustomValidators.cnpj
      ]],
      senha: [null, [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(6)
      ]],
      senhaConfirmacao: [null, [
        Validators.required,
        MyCustomValidators.equalTo('senha')
      ]],
    });

    this.registerForm.get('senha')?.valueChanges.subscribe((valor: string) => {
      this.validarRequisitos(valor);
    });
  }

  validarRequisitos(valor: string) {
    this.requisitosSenha.minuscula = /[a-z]/.test(valor);
    this.requisitosSenha.maiuscula = /[A-Z]/.test(valor);
    this.requisitosSenha.numero = /\d/.test(valor);
    this.requisitosSenha.caracterEspecial = /[!@#$%^&*(),.?":{}|<>]/.test(valor);
    this.requisitosSenha.minimo = valor?.length >= 6;
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.registerForm);
  }

  limparErros() {
    this.errors = [];
  }

  efetuarRegistro() {
    super.validarFormulario(this.registerForm)

    if (this.registerForm.dirty && this.registerForm.valid) {

      this.instituicao = Object.assign({}, this.instituicao, this.registerForm.value);

      this.identidadeService.registrar(this.instituicao)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarErro(falha); }
        });
    }
  }

  processarSucesso(response: any) {
    this.registerForm.reset();
    this.limparErros();

    let email = this.instituicao.email;
    let usuario = this.instituicao.usuarioNome;

    this.router.navigate(
      ['/confirmar-email-enviado'],
      { queryParams: { email, usuario }}
    );
  }

  processarErro(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
  }

  closeAlert() {
    this.limparErros();
  }
}
