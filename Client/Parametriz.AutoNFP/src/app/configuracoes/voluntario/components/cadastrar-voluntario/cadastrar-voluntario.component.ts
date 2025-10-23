import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { VoluntarioService } from '../../services/voluntario.service';
import { ToastrService } from 'ngx-toastr';
import { Voluntario } from '../../models/voluntario';

@Component({
  selector: 'app-cadastrar-voluntario',
  standalone: false,
  templateUrl: './cadastrar-voluntario.component.html',
  styles: ``
})
export class CadastrarVoluntarioComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  cadastrarVoluntarioForm!: FormGroup;
  voluntario!: Voluntario;

  errors: [] = [];

  constructor(
    private formBuilder: FormBuilder,
    private activeModal: NgbActiveModal,
    private voluntarioService: VoluntarioService,
    private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      nome: { required: 'Nome é obrigatório' },
      email: {
        required: 'E-mail é obrigatório.',
        email: 'O e-mail informado não é válido.'
      },
      cpf: { required: 'CPF é obrigatório'},
      contato: { required: 'Contato é obrigatório '}
    }

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastrarVoluntarioForm);
  }

  ngOnInit(): void {
    this.cadastrarVoluntarioForm = this.formBuilder.group({
      nome: ['', [ Validators.required ]],
      email:  ['', [Validators.required, Validators.email]],
      cpf: ['', Validators.required],
      contato: ['', Validators.required],
      administrador: [false]
    ,
    });
  }

  efetuarCadastrarVoluntario() {
    super.validarFormulario(this.cadastrarVoluntarioForm);

    if (this.cadastrarVoluntarioForm.dirty && this.cadastrarVoluntarioForm.valid) {

      this.voluntario = Object.assign({}, this.voluntario, this.cadastrarVoluntarioForm.value);

      console.log(this.voluntario)

      this.voluntarioService.cadastrar(this.voluntario)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarFalha(falha); }
        })
    }
  }

  processarSucesso() {
    this.limparErros()

    this.toastr.success('Voluntário cadastrado com sucesso.', 'Sucesso!');
    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível cadastrar o voluntário.', 'Erro');
  }

  fecharModal() {
    this.activeModal.close();
  }

  limparErros() {
    this.errors = [];
  }
}
