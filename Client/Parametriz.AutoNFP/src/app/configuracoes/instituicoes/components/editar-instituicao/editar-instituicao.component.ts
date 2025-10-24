import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { BaseFormComponent } from 'src/app/shared/generic-form-validator/base-form.component';
import { Instituicao } from '../../models/instituicao';
import { InputUtils } from 'src/app/shared/utils/input-utils';
import { InstituicaoService } from '../../services/instituicao.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-editar-instituicao',
  standalone: false,
  templateUrl: './editar-instituicao.component.html',
  styles: ``
})
export class EditarInstituicaoComponent extends BaseFormComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  editarInstituicaoForm!: FormGroup;
  errors: any[] = [];
  instituicao!: Instituicao;

  inputUtils = InputUtils;

  constructor(
    private formBuilder: FormBuilder,
    private instituicaoService: InstituicaoService,
    private toastr: ToastrService
  ) {
    super();

    this.validationMessages = {
      razaoSocial: {
        required: 'A razão social é obrigatória.'
      },
      entidadeNomeNFP: {
        required: 'O nome da entidade na NFP é obrigatório.'
      },
      logradouro: {
        required: 'O logradouro é obrigatório.'
      },
      numero: {
        required: 'O número é obrigatório.'
      },
      bairro: {
        required: 'O bairro é obrigatório.'
      },
      cep: {
        required: 'O CEP é obrigatório.'
      },
      municipio: {
        required: 'O município é obrigatório.'
      },
      uf: {
        required: 'A UF é obrigatória.'
      }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.editarInstituicaoForm);
  }

  ngOnInit(): void {
    this.obterInstituicao();

    this.editarInstituicaoForm = this.formBuilder.group({
      razaoSocial: ['', Validators.required],
      entidadeNomeNFP: ['', Validators.required],
      endereco: this.formBuilder.group({
        logradouro: [null, Validators.required],
        numero: [null, Validators.required],
        complemento: [null],
        bairro: [null, Validators.required],
        cep: [null, Validators.required],
        municipio: [null, Validators.required],
        uf: [null, Validators.required],
      })
    });
  }

  obterInstituicao() {
    this.instituicaoService.obter()
      .subscribe({
        next: (insitituicao: Instituicao) => {
          this.instituicao = insitituicao
          this.preencherForm(this.instituicao);
        },
        error: () => this.toastr.error('Erro ao carregar instituição', 'Erro')
      })
  }

  preencherForm(insitituicao: Instituicao) {
    this.editarInstituicaoForm.patchValue(insitituicao);
  }

  efetuarEditarInstituicao() {
    this.validarFormulario(this.editarInstituicaoForm);

    if (this.editarInstituicaoForm.dirty && this.editarInstituicaoForm.value) {

      this.instituicao = Object.assign({}, this.instituicao, this.editarInstituicaoForm.value);

      this.instituicaoService.editar(this.instituicao)
        .subscribe({
          next: () => { this.processarSucesso(); },
          error: (falha: any) => { this.processarFalha(falha); }
        });
    }
  }

  processarSucesso() {
    this.limparErros();
    this.toastr.success('Instituição alterada com sucesso!', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao editar instituição.', 'Erro!');
  }

  limparErros() {
    this.errors = [];
  }
}