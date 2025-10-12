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

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { VoluntarioComponent } from './voluntario/voluntario.component';
import { CadastrarVoluntarioComponent } from './voluntario/cadastrar-voluntario/cadastrar-voluntario.component';
import { VisualizarVoluntarioComponent } from './voluntario/visualizar-voluntario/visualizar-voluntario.component';

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';



import { AlertModule } from 'ngx-bootstrap/alert';


import { UsuarioComponent } from './usuario/usuario.component';
import { CadastrarUsuarioComponent } from './usuario/components/cadastrar-usuario/cadastrar-usuario.component';
import { ListarUsuarioComponent } from './usuario/components/listar-usuario/listar-usuario.component';
import { DesativarUsuarioComponent } from './usuario/components/desativar-usuario/desativar-usuario.component';
import { AtivarUsuarioComponent } from './usuario/components/ativar-usuario/ativar-usuario.component';
import { EditarUsuarioComponent } from './usuario/components/editar-usuario/editar-usuario.component';
import { PefilUsuarioComponent } from './usuario/components/pefil-usuario/pefil-usuario.component';
import { VoluntarioService } from './voluntario/services/voluntario.service';



@NgModule({
  declarations: [
    ConfiguracoesComponent,
    VoluntarioComponent,
    CadastrarVoluntarioComponent,
    VisualizarVoluntarioComponent,
    UsuarioComponent,
    CadastrarUsuarioComponent,
    ListarUsuarioComponent,
    DesativarUsuarioComponent,
    AtivarUsuarioComponent,
    EditarUsuarioComponent,
    PefilUsuarioComponent,
  ],
  imports: [
    CommonModule,
    ConfiguracoesRoutingModule,
    NgbModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    MatSelectModule,
    AlertModule,
    MatInputModule,
    MatMenuModule
  ],
  providers: [
    UsuarioService,
    VoluntarioService,
    AutorizacaoService,
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        jwtInterceptor
      ])),
  ]
})
export class ConfiguracoesModule { }
