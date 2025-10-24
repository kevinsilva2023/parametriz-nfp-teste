import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cpf',
  standalone: true
})
export class CpfPipe implements PipeTransform {

  transform(value: string): string {

    let valueTranform = value?.replace(/\D/g, '');
    
    let mascara = /^(\d{3})(\d{3})(\d{3})(\d{2})$/;

    let result = valueTranform?.replace(mascara, '$1.$2.$3-$4');

    return result;
  }

}
