import { Component } from '@angular/core';
import { ErroTransmissaoLote } from '../models/erro-transmissao-lote';

@Component({
  selector: 'app-listar-erro-modal',
  standalone: false,
  templateUrl: './listar-erro-modal.component.html',
  styles: ``
})
export class ListarErroModalComponent {
  erroTransmissaoLote = [
  {
    data: '2025-10-24 10:15:00',
    mensagem: 'Falha ao conectar com o servidor SEFAZ. Verifique a conexão e tente novamente.',
    voluntario: 'kevin silva'
  },
  {
    data: '2025-10-24 10:18:32',
    mensagem: 'Certificado digital expirado. Atualize o certificado antes de reenviar o lote.',
    voluntario: 'kevin silva'
  },
  {
    data: '2025-10-24 10:21:05',
    mensagem: 'Formato do arquivo XML inválido. Corrija o layout e reenvie.',
    voluntario: 'kevin silva'
  }
];
}
