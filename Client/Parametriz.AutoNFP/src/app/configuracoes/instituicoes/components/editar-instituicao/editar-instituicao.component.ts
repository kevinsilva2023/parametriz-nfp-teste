import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup } from '@angular/forms';
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

    }

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.editarInstituicaoForm);
  }

  ngOnInit(): void {
    this.editarInstituicaoForm = this.formBuilder.group({
      
    })
  }
}