import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { DefinirSenha } from '../../models/definir-senha';
import { EnviarDefinirSenha } from '../../models/enviar-definir-senha';
import { IdentidadeService } from '../../services/identidade.service';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-esqueceu-a-senha',
  standalone: false,
  templateUrl: './esqueceu-a-senha.component.html',
})
export class EsqueceuASenhaComponent extends BaseFormComponent implements OnInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  esqueceuASenhaForm!: FormGroup
  enviarDefinirSenha!: EnviarDefinirSenha;
  errors: any[] = [];

  constructor(private formBuilder: FormBuilder,
              private identidadeService: IdentidadeService,
              private activateRoute: ActivatedRoute,
              private router: Router,
              private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      email: { required: 'Favor preencher e-mail.'}
    }

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {
    this.esqueceuASenhaForm = this.formBuilder.group({
      email: [null, Validators.required]
    })
    this.preencherForm();
  }

  preencherForm() {
    const email = this.activateRoute.snapshot.queryParamMap.get('email');
    if (email) {
      this.esqueceuASenhaForm.patchValue({ email });
      this.esqueceuASenhaForm.markAsDirty(); // 
    }
  }

  enviarEmailRecuperacaoDeSenha() {
    this.esqueceuASenhaForm.markAllAsTouched();

    if (this.esqueceuASenhaForm.dirty && this.esqueceuASenhaForm.valid) {

      this.enviarDefinirSenha = Object.assign({}, this.enviarDefinirSenha, this.esqueceuASenhaForm.value);

      this.identidadeService.enviarDefinirSenha(this.enviarDefinirSenha)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        })
    }
  }

  limparErros() {
    this.errors = [];
  }

  processarSucesso(response: any) {
    this.esqueceuASenhaForm.reset();
    this.limparErros();

    LocalStorageUtils.limparDadosLocaisUsuario();

    let toast = this.toastr.success('Verifique sua caixa de entrada e siga as instruções.', 'Email enviado!');
    let email = this.enviarDefinirSenha.email;

   this.router.navigate(
          ['/definir-senha-enviado'],
          { queryParams: { email }}
        )
  }

  processarFalha(fail: any){
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível enviar o email.', 'Erro!');
  }
}
