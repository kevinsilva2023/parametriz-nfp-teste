import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { Voluntario } from '../../models/voluntario';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VoluntarioService } from '../../services/voluntario.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-editar-voluntario',
  standalone: false,
  templateUrl: './editar-voluntario.component.html',
  styles: ``
})
export class EditarVoluntarioComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  voluntarioId!: string;
  voluntario!: Voluntario;

  editarVoluntarioForm!: FormGroup;

  errors: [] = [];

  constructor(
    private formBuilder: FormBuilder,
    private activeModal: NgbActiveModal,
    private voluntarioService: VoluntarioService,
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
    super.configurarValidacaoFormularioBase(this.formInputElements, this.editarVoluntarioForm);
  }

  ngOnInit(): void {
    // vai poder editar nome e telefone apenas
    this.editarVoluntarioForm = this.formBuilder.group({
      nome: ['', [
        Validators.required
      ]],
      administrador: [false]
    });
    this.obterPorId();
  }

  obterPorId() {
    this.voluntarioService.obterPorId(this.voluntarioId)
      .subscribe({
        next: (response: Voluntario) => {
          this.voluntario = response;
          this.preencherForm();
        },
        error: () => this.toastr.error('Erro ao obter usuário.', 'Erro')
      });
  }

  preencherForm() {
    this.editarVoluntarioForm.patchValue({
      nome: this.voluntario.nome,
      administrador: this.voluntario.administrador
    });
  }

  efetuarEditarVoluntario() {
    super.validarFormulario(this.editarVoluntarioForm);

    if (this.editarVoluntarioForm.dirty && this.editarVoluntarioForm.valid) {

      this.voluntario = Object.assign({}, this.voluntario, this.editarVoluntarioForm.value);

      this.voluntarioService.editar(this.voluntario)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarFalha(falha); }
        })
    }
  }

  processarSucesso() {
    this.limparErros()

    this.toastr.success('Voluntário editado com sucesso.', 'Sucesso!');
    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível editar o voluntário.', 'Erro');
  }

  fecharModal() {
    this.activeModal.close();
  }

  limparErros() {
    this.errors = [];
  }
}
