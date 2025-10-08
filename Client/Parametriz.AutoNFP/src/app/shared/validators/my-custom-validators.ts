import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class MyCustomValidators {

  static cpf(control: AbstractControl): ValidationErrors | null {
    const numeros = control.value;

    const regex = new RegExp('[0-9]{11}');
    if (!regex.test(numeros))
      return { cpf: true };

    let igual: boolean = true;
    for (let i = 1; i < 11; i++)
      if (numeros[i] != numeros[0])
        igual = false;

    if (igual || numeros == "12345678909")
      return { cpf: true };

    let soma: number = 0;
    let resultado: number;

    for (let i = 0; i < 9; i++)
      soma += (10 - i) * parseInt(numeros[i]);

    resultado = soma % 11;

    if (resultado == 1 || resultado == 0) {
      if (parseInt(numeros[9]) != 0)
        return { cpf: true };
    } else {
      if (parseInt(numeros[9]) != (11 - resultado))
        return { cpf: true };
    }

    soma = 0;
    for (let i = 0; i < 10; i++)
      soma += (11 - i) * parseInt(numeros[i]);

    resultado = soma % 11;

    if (resultado == 1 || resultado == 0) {
      if (parseInt(numeros[10]) != 0)
        return { cpf: true };
    } else {
      if (parseInt(numeros[10]) != 11 - resultado)
        return { cpf: true };
    }

    return null;
  }

  static cnpj(control: AbstractControl): ValidationErrors | null {

    const numeros = control.value;

    const regex = new RegExp('[0-9]{14}');
    if (!regex.test(numeros))
      return { cnpj: true };

    let igual: boolean = true;

    for (let i = 1; i < 14; i++)
      if (numeros[i] != numeros[0])
        igual = false;

    if (igual)
      return { cnpj: true };

    let numerosVerificacao = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    let resultadoMultiplicacao = 0;

    for (let i = 0; i < 12; i++)
      resultadoMultiplicacao += numeros[i] * numerosVerificacao[i + 1];

    let resultadoVerificacao = resultadoMultiplicacao % 11;

    if (resultadoVerificacao < 2) {
      if (numeros[12] != 0)
        return { cnpj: true };
    } else {
      if (numeros[12] != (11 - resultadoVerificacao))
        return { cnpj: true };
    }

    resultadoMultiplicacao = 0;

    for (var i = 0; i < 13; i++)
      resultadoMultiplicacao += numeros[i] * numerosVerificacao[i];

    resultadoVerificacao = resultadoMultiplicacao % 11;
    if (resultadoVerificacao < 2) {
      if (numeros[13] != 0)
        return { cnpj: true };
    } else {
      if (numeros[13] != (11 - resultadoVerificacao))
        return { cnpj: true };
    }

    return null;
  }

  static equalTo(outroCampo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const fieldValue = control.value;
      const otherFieldValue = control.root.get(outroCampo)?.value;

      if (fieldValue !== otherFieldValue) {
        return { equalTo: true };
      }
      return null;
    }
  }
}
