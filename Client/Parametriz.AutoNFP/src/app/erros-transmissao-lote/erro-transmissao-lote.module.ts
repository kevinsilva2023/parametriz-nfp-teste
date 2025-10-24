import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ListarErroTransmissaoLoteModalComponent } from './listar-erro-transmissao-lote-modal/listar-erro-transmissao-lote-modal.component';

@NgModule({
  declarations: [
    ListarErroTransmissaoLoteModalComponent
  ],
  imports: [
    CommonModule,
    NgbModule
  ]
})
export class ErroTransmissaoLoteModule { }
