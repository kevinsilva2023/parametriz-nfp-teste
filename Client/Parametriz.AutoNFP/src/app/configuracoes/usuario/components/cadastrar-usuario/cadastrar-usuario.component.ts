import { AfterViewInit, Component, ElementRef, Input, input, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { Usuario } from '../../models/usuario';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { UsuarioService } from '../../services/usuario.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cadastrar-usuario',
  standalone: false,
  templateUrl: './cadastrar-usuario.component.html',
  styles: ``
})
export class CadastrarUsuarioComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];
  
  cadastroUsuarioForm!: FormGroup;
  usuario!: Usuario;

  errors:[] = [];

  constructor(private formBuilder: FormBuilder,
              private activeModal: NgbActiveModal,
              private usuarioService: UsuarioService,
              private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      nome: {
        required: 'Favor preencher o nome.'
      },
      email: {
        required: 'Favor preencher o e-mail.',
        email: 'O e-mail informado não é válido.'
      },
    }

    super.configurarMensagensValidacaoBase(this.validationMessages); 
  }
  
  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastroUsuarioForm);
  }

  ngOnInit(): void {
    this.cadastroUsuarioForm = this.formBuilder.group({
      nome: ['', [
        Validators.required
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      administrador: [false]
    });
  }
  
  efetuarCadastrarUsuario() {
    super.validarFormulario(this.cadastroUsuarioForm);

    let instituicaoId = LocalStorageUtils.obterInstituicaoId();

    if(this.cadastroUsuarioForm.dirty && this.cadastroUsuarioForm.valid) {

      this.usuario = Object.assign({}, this.usuario, {
        // Verificar outra possibilidade
        nome: this.cadastroUsuarioForm.get('nome')?.value,
        email: { conta: this.cadastroUsuarioForm.get('email')?.value},
        administrador: this.cadastroUsuarioForm.get('administrador')?.value,
        instituicaoId: instituicaoId
      });
      
      console.log(this.usuario)

      this.usuarioService.cadastrar(this.usuario)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        })
    }
  }

  processarSucesso(response: any) {
    this.limparErros()
    
    this.toastr.success('Usuário cadastrado com sucesso.','Sucesso!');
    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível cadastrar o usuário.', 'Erro');
  }

  fecharModal() {
    this.activeModal.close();
  }

  limparErros() {
    this.errors = [];
  }
}
