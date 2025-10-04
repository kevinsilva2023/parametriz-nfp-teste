import { ElementRef } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { fromEvent, merge, Observable } from 'rxjs';

import { GenericFormValidator } from './generic-form-validator';
import { DisplayMessage } from './display-message';
import { ValidationMessages } from './validation-messages';

export abstract class BaseFormComponent {

  displayMessage: DisplayMessage = {};
  genericFormValidator: GenericFormValidator | null = null;

  protected configurarMensagensValidacaoBase(validationMessages: ValidationMessages) {
    this.genericFormValidator = new GenericFormValidator(validationMessages);
  }

  protected configurarValidacaoFormularioBase(formInputElements: ElementRef[], formGroup: FormGroup) {
    let controlBlurs: Observable<any>[] = formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.validarFormulario(formGroup)
    });
  }

  protected validarFormulario(formGroup: FormGroup) {
    this.displayMessage = this.genericFormValidator?.processarMensagens(formGroup) ?? {};
  }
}
