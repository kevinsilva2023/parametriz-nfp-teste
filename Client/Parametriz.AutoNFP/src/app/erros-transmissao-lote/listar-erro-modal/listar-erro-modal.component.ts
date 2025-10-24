import { Component } from '@angular/core';
import { ErroTransmissaoLote } from '../models/erro-transmissao-lote';

@Component({
  selector: 'app-listar-erro-modal',
  standalone: false,
  templateUrl: './listar-erro-modal.component.html',
  styles: ``
})
export class ListarErroModalComponent {
  errosTransmissaoLote!: ErroTransmissaoLote[];
}
