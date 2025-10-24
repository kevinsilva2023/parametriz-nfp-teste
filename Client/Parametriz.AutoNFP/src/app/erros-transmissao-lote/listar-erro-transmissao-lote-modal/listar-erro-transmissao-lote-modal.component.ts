import { Component } from '@angular/core';
import { ErroTransmissaoLote } from '../models/erro-transmissao-lote';

@Component({
  selector: 'app-listar-erro-transmissao-lote-modal',
  standalone: false,
  templateUrl: './listar-erro-transmissao-lote-modal.component.html',
  styles: ``
})
export class ListarErroTransmissaoLoteModalComponent {
  errosTransmissaoLote!: ErroTransmissaoLote[];
}
