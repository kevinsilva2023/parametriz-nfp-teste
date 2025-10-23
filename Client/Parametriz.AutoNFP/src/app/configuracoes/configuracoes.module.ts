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

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';

import { UsuarioComponent } from './usuarios/usuario.component';
import { CadastrarUsuarioComponent } from './usuarios/components/cadastrar-usuario/cadastrar-usuario.component';
import { ListarUsuarioComponent } from './usuarios/components/listar-usuario/listar-usuario.component';
import { DesativarUsuarioComponent } from './usuarios/components/desativar-usuario/desativar-usuario.component';
import { AtivarUsuarioComponent } from './usuarios/components/ativar-usuario/ativar-usuario.component';
import { EditarUsuarioComponent } from './usuarios/components/editar-usuario/editar-usuario.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { InstituicaoComponent } from './instituicoes/instituicao.component';
import { CadastrarVoluntarioComponent } from './voluntario/components/cadastrar-voluntario/cadastrar-voluntario.component';
import { EditarVoluntarioComponent } from './voluntario/components/editar-voluntario/editar-voluntario.component';
import { AtivarVoluntarioComponent } from './voluntario/components/ativar-voluntario/ativar-voluntario.component';
import { DesativarVoluntarioComponent } from './voluntario/components/desativar-voluntario/desativar-voluntario.component';
import { ListarVoluntarioComponent } from './voluntario/components/listar-voluntario/listar-voluntario.component';


@NgModule({
  declarations: [
    ConfiguracoesComponent,
    UsuarioComponent,
    CadastrarUsuarioComponent,
    ListarUsuarioComponent,
    DesativarUsuarioComponent,
    AtivarUsuarioComponent,
    EditarUsuarioComponent,
    InstituicaoComponent,
    CadastrarVoluntarioComponent,
    EditarVoluntarioComponent,
    AtivarVoluntarioComponent,
    DesativarVoluntarioComponent,
    ListarVoluntarioComponent
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
    MatMenuModule,
    MatTooltipModule
  ],
  providers: [
    UsuarioService,
    AutorizacaoService,
  
  ]
})
export class ConfiguracoesModule { }
