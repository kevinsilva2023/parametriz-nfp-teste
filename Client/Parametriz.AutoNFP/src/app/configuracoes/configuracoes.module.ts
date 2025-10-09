import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfiguracoesRoutingModule } from './configuracoes-routing.module';
import { ConfiguracoesComponent } from './configuracoes.component';

import { VoluntarioComponent } from './components/voluntario/voluntario.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    ConfiguracoesComponent,
    VoluntarioComponent
  ],
  imports: [
    CommonModule,
    ConfiguracoesRoutingModule,
    NgbModule,
]
})
export class ConfiguracoesModule { }
