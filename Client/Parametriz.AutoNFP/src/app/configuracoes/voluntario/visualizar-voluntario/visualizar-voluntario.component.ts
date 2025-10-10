import { Component } from '@angular/core';

@Component({
  selector: 'app-visualizar-voluntario',
  standalone: false,
  templateUrl: './visualizar-voluntario.component.html'
})
export class VisualizarVoluntarioComponent {
  dadosCertificadoDigital = {
    nome: 'Kevin Marcos Pereira da Silva',
    cpf: '40575854863',
    validade: '30/10/2025',
    diasParaVencimento: '20',
    emissor: 'SEFAZ AB/IP',
    status: 'Ativo'
  };
}
