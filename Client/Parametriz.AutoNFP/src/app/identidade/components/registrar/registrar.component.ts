import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, viewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
// import { Instituicao } from '../../models/cadastrar-instituicao';
import { IdentidadeService } from '../../services/identidade.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { MyCustomValidators } from 'src/app/shared/validators/my-custom-validators';
import { debounceTime, Subject } from 'rxjs';
import { InputUtils } from 'src/app/shared/utils/input-utils';

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
  // instituicao!: Instituicao

  inputUtils = InputUtils;
  debounceCnpj = new Subject<string>();

  constructor(
    private formBuilder: FormBuilder,
    private identidadeService: IdentidadeService,
    private router: Router
  ) {
    super();

    this.validationMessages = {
      cnpj: {
        required: 'Favor preencher o CNPJ.',
        cnpj: 'O CNPJ informado é inválido.'
      }
    };

    LocalStorageUtils.limparDadosLocaisUsuario();

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      cnpj: [null, [
        Validators.required,
        MyCustomValidators.cnpj
      ]]
    });

    this.debounceCnpj
      .pipe(debounceTime(2000))
      .subscribe({
        next: (cnpj: string) => {
          this.obterCnpj(cnpj)
        }
      });
  }

  obterCnpj(cnpj: string) {
    this.identidadeService.obterDadosCnpj(cnpj)
      .subscribe({
        next: (response) => console.log(response),
        error: (err) => console.log(err)
      })
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.registerForm);
  }

  // efetuarRegistro() {
  //   super.validarFormulario(this.registerForm)

  //   if (this.registerForm.dirty && this.registerForm.valid) {

  //     this.instituicao = Object.assign({}, this.instituicao, this.registerForm.value);

  //     this.identidadeService.registrar(this.instituicao)
  //       .subscribe({
  //         next: () => { this.processarSucesso(); },
  //         error: (falha: any) => { this.processarErro(falha); }
  //       });
  //   }
  // }

  // processarSucesso() {
  //   this.registerForm.reset();
  //   this.limparErros();

  //   let email = this.instituicao.email;
  //   let usuario = this.instituicao.usuarioNome;

  //   this.router.navigate(
  //     ['/confirmar-email-enviado'],
  //     { queryParams: { email, usuario } }
  //   );
  // }

  processarErro(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
  }

  limparErros() {
    this.errors = [];
  }

}
