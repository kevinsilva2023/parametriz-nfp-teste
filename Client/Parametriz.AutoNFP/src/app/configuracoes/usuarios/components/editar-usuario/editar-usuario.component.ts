import { AfterViewInit, Component, ElementRef, Input, input, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { Usuario } from '../../models/usuario';
import { UsuarioService } from '../../services/usuario.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-editar-usuario',
  standalone: false,
  templateUrl: './editar-usuario.component.html',
  styles: ``
})
export class EditarUsuarioComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  usuarioId!: string;
  usuario!: Usuario;

  editarUsuarioForm!: FormGroup;

  errors: [] = [];

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
    super.configurarValidacaoFormularioBase(this.formInputElements, this.editarUsuarioForm);
  }

  ngOnInit(): void {
    this.editarUsuarioForm = this.formBuilder.group({
      nome: ['', [
        Validators.required
      ]],
      administrador: [false]
    });
    this.obterPorId();
  }

  obterPorId() {
    this.usuarioService.obterPorId(this.usuarioId)
      .subscribe({
        next: (response: Usuario) => {
          this.usuario = response;
          this.preencherForm();
        },
        error: (erro: any) => this.toastr.error('Erro ao obter usuário.', 'Erro')
      });
  }

  preencherForm() {
    this.editarUsuarioForm.patchValue({
      nome: this.usuario.nome,
      administrador: this.usuario.administrador
    });
  }

  efetuarEditarUsuario() {
    super.validarFormulario(this.editarUsuarioForm);

    if (this.editarUsuarioForm.dirty && this.editarUsuarioForm.valid) {

      this.usuario = Object.assign({}, this.usuario, this.editarUsuarioForm.value);

      this.usuarioService.editar(this.usuario)
        .subscribe({
          next: (sucesso: any) => { this.processarSucesso(sucesso); },
          error: (falha: any) => { this.processarFalha(falha); }
        })
    }
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Usuário editado com sucesso.', 'Sucesso!');
    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível editar o usuário.', 'Erro');
  }

  fecharModal() {
    this.activeModal.close();
  }

  limparErros() {
    this.errors = [];
  }
}
