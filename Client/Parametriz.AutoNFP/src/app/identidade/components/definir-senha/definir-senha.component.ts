import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { DefinirSenha } from '../../models/definir-senha';
import { IdentidadeService } from '../../services/identidade.service';
import { MyCustomValidators } from 'src/app/shared/validators/my-custom-validators';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-definir-senha',
  standalone: false,
  templateUrl: './definir-senha.component.html',
})
export class DefinirSenhaComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  definirSenhaForm!: FormGroup;
  definirSenha!: DefinirSenha;
  errors: any[] = [];

  verSenha = false;
  verSenhaConfirmacao = false;

  returnUrl: string;

  emailDefinirSenha?: string;
  codeDefinirSenha?: string;

  requisitosSenha = {
    minuscula: false,
    maiuscula: false,
    numero: false,
    caracterEspecial: false,
    minimo: false
  }

  constructor(private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private identidadeService: IdentidadeService,
    private toastr: ToastrService) {

    super();
    this.validationMessages = {
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

    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];

    super.configurarMensagensValidacaoBase(this.validationMessages);

  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.definirSenhaForm);
  }

  ngOnInit(): void {
    this.definirSenhaForm = this.formBuilder.group({
      senha: ['', [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(6)
      ]],
      senhaConfirmacao: ['', [
        Validators.required,
        MyCustomValidators.equalTo('senha')
      ]]
    });

    this.definirSenhaForm.get('senha')?.valueChanges.subscribe((valor: string) => {
      this.validarRequisitos(valor);
    });

    this.caputurarDadosParaDefinirSenha();
  }

  validarRequisitos(valor: string) {
    this.requisitosSenha.minuscula = /[a-z]/.test(valor);
    this.requisitosSenha.maiuscula = /[A-Z]/.test(valor);
    this.requisitosSenha.numero = /\d/.test(valor);
    this.requisitosSenha.caracterEspecial = /[!@#$%^&*(),.?":{}|<>]/.test(valor);
    this.requisitosSenha.minimo = valor?.length >= 6;
  }


  limparErros() {
    this.errors = [];
  }

  caputurarDadosParaDefinirSenha() {
    const email = this.activatedRoute.snapshot.queryParamMap.get('email')
    const code = this.activatedRoute.snapshot.queryParamMap.get('code')

    if (email && code !== null) {
      this.emailDefinirSenha = email;
      this.codeDefinirSenha = code;
    }
  }

  efetuarDefinirSenha() {
    super.validarFormulario(this.definirSenhaForm)

    if (this.definirSenhaForm.dirty && this.definirSenhaForm.valid) {

      this.definirSenha = Object.assign({}, this.definirSenha, this.definirSenhaForm.value, {
        email: this.emailDefinirSenha,
        code: this.codeDefinirSenha
      })

      this.identidadeService.definirSenha(this.definirSenha)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        });
    }
  }

  processarSucesso(response: any) {
    this.definirSenhaForm.reset();
    this.limparErros();

    LocalStorageUtils.salvarDadosLocaisUsuario(response);

    this.toastr.success(
      'Aguarde, você será redirecionado para o AutoNFP...',
      'Senha definida com sucesso!',
      {
        timeOut: 5000,
        progressBar: true,
        progressAnimation: 'increasing',
      }
    );

    setTimeout(() => {
      this.returnUrl
        ? this.router.navigate([this.returnUrl])
        : this.router.navigate(['/']);
    }, 5000);
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível definir a senha.', 'Erro', {
      timeOut: 5000,
      progressBar: true,
      progressAnimation: 'increasing',
    });
  }
}
