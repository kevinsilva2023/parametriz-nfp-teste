import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, viewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Instituicao } from '../../models/instituicao';
import { IdentidadeService } from '../../services/identidade.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { supportsPassiveEventListeners } from '@angular/cdk/platform';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrl: './registrar.component.scss',
  standalone: false
})
export class RegistrarComponent extends BaseFormComponent implements OnInit, AfterViewInit{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  registerForm!: FormGroup;
  errors: any[] = [];
  instituicao!: Instituicao
  verSenha = false;

  returnUrl: string;

  constructor(private formBuilder: FormBuilder,
              private identidadeService: IdentidadeService,
              private activatedRoute: ActivatedRoute,
              private router: Router) {
    super();

    this.validationMessages = {
      razaoSocial: { required: 'favor preencher razao' },
      email: { required: 'favor preencher email' },
      voluntarioNome: { required: 'favor preencher voluntarioNome'},
      cnpj: { required: 'favor preencher cnpj'},
      senha: { required: 'favor preencher senha'},
      senhaConfirmacao: { required: 'favor preencher senhaConfirmacao'},
    };

    LocalStorageUtils.limparDadosLocaisUsuario();

    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      razaoSocial: [null, Validators.required],
      email: [null, Validators.required],
      voluntarioNome: [null, Validators.required],
      cnpj: [null, Validators.required],
      senha: [null, Validators.required],
      senhaConfirmacao: [null, Validators.required],
    });
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.registerForm);
  }

  limparErros() {
    this.errors = [];
  }

  efetuarRegistro() {
    this.registerForm.markAllAsTouched();
    this.displayMessage = this.genericFormValidator.processarMensagens(this.registerForm);

    if(this.registerForm.invalid) return;

    if(this.registerForm.dirty && this.registerForm.valid) {

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

    this.returnUrl ? this.router.navigate([this.returnUrl]) : this.router.navigate(['/confirmar-email']);
  }

  processarErro(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
  }

  closeAlert() {
    this.limparErros();
  }
}
