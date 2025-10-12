import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { IdentidadeService } from '../../services/identidade.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Login } from '../../models/login';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  errors: any[] = [];
  loginForm!: FormGroup;
  login!: Login;
  verSenha = false;

  returnUrl: string;

  constructor(private formBuilder: FormBuilder,
              private identidadeService: IdentidadeService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private toastr: ToastrService) 
  {
    super();
    
    this.validationMessages = {
      email: {
        required: 'Favor preencher o e-mail.',
        email: 'E-mail inválido.'
      },
      senha: {
        required: 'Favor preencher a senha.',
        minlength: 'A senha deve ser preenchida com no mínimo 6 caracteres.'
      }
    };

    LocalStorageUtils.limparDadosLocaisUsuario();

    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {    
    this.loginForm = this.formBuilder.group({
      email: [null, [
        Validators.required,
        Validators.email
      ]],
      senha: [null, [
        Validators.required,
        Validators.minLength(6)
      ]]
    });
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.loginForm);
  }

  limparErros() {
    this.errors = [];
  }

  efetuarLogin() {
    if (this.loginForm.dirty && this.loginForm.valid) {

      this.login = Object.assign({}, this.login, this.loginForm.value);

      this.identidadeService.login(this.login)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        });
    }
  }

  processarSucesso(response: any) {
    this.limparErros();
    this.loginForm.reset();
    
    LocalStorageUtils.salvarDadosLocaisUsuario(response);

    console.log(this.returnUrl);

    this.returnUrl ? this.router.navigate([this.returnUrl]) : this.router.navigate(['/']);

    this.toastr.success('Login realizado com sucesso', 'Sucesso!')

  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Usuário ou senha incorretos.', 'Erro de Login!');
  }

  closeAlert() {
    this.limparErros();
  }
}
