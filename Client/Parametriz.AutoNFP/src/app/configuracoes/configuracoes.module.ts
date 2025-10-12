import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfiguracoesRoutingModule } from './configuracoes-routing.module';
import { ConfiguracoesComponent } from './configuracoes.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from 'src/app/shared/interceptors/error.interceptor';
import { jwtInterceptor } from 'src/app/shared/interceptors/jwt.interceptor';
import { UsuarioService } from './usuario/services/usuario.service';

@NgModule({
  declarations: [
    ConfiguracoesComponent,
  ],
  imports: [
    CommonModule,
    ConfiguracoesRoutingModule,
    NgbModule,
  ],
  providers: [
    UsuarioService,
    AutorizacaoService,
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        jwtInterceptor
      ])),
  ]
})
export class ConfiguracoesModule { }
