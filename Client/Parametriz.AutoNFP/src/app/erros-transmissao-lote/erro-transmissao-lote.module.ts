import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListarErroModalComponent } from './listar-erro-modal/listar-erro-modal.component';
import {  NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    ListarErroModalComponent
  ],
  imports: [
    CommonModule,
    NgbModule
  ]
})
export class ErroTransmissaoLoteModule { }
