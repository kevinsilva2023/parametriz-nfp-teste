import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfiguracoesRoutingModule } from './configuracoes-routing.module';
import { ConfiguracoesComponent } from './configuracoes.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from 'src/app/shared/interceptors/error.interceptor';
import { jwtInterceptor } from 'src/app/shared/interceptors/jwt.interceptor';
import { UsuarioService } from './usuarios/services/usuario.service';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { VoluntarioComponent } from './voluntarios/voluntario.component';
import { CadastrarVoluntarioComponent } from './voluntarios/components/cadastrar-voluntario/cadastrar-voluntario.component';
import { VisualizarVoluntarioComponent } from './voluntarios/components/visualizar-voluntario/visualizar-voluntario.component';

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';

import { UsuarioComponent } from './usuarios/usuario.component';
import { CadastrarUsuarioComponent } from './usuarios/components/cadastrar-usuario/cadastrar-usuario.component';
import { ListarUsuarioComponent } from './usuarios/components/listar-usuario/listar-usuario.component';
import { DesativarUsuarioComponent } from './usuarios/components/desativar-usuario/desativar-usuario.component';
import { AtivarUsuarioComponent } from './usuarios/components/ativar-usuario/ativar-usuario.component';
import { EditarUsuarioComponent } from './usuarios/components/editar-usuario/editar-usuario.component';
import { VoluntarioService } from './voluntarios/services/voluntario.service';



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
