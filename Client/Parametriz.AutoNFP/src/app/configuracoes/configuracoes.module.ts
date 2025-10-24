import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfiguracoesRoutingModule } from './configuracoes-routing.module';
import { ConfiguracoesComponent } from './configuracoes.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';

import { MatTooltipModule } from '@angular/material/tooltip';
import { InstituicaoComponent } from './instituicoes/instituicao.component';

import { VoluntarioComponent } from './voluntario/voluntario.component';
import { CadastrarVoluntarioComponent } from './voluntario/components/cadastrar-voluntario/cadastrar-voluntario.component';
import { AtivarVoluntarioComponent } from './voluntario/components/ativar-voluntario/ativar-voluntario.component';
import { DesativarVoluntarioComponent } from './voluntario/components/desativar-voluntario/desativar-voluntario.component';
import { ListarVoluntarioComponent } from './voluntario/components/listar-voluntario/listar-voluntario.component';
import { VoluntarioService } from './voluntario/services/voluntario.service';
import { CpfPipe } from '../shared/pipe/cpf.pipe';

import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { EditarInstituicaoComponent } from './instituicoes/components/editar-instituicao/editar-instituicao.component';
import { InstituicaoService } from './instituicoes/services/instituicao.service';
import { CnpjPipe } from '../shared/pipe/cnpj.pipe';

@NgModule({
  declarations: [
    ConfiguracoesComponent,
    VoluntarioComponent,
    InstituicaoComponent,
    CadastrarVoluntarioComponent,
    AtivarVoluntarioComponent,
    DesativarVoluntarioComponent,
    ListarVoluntarioComponent,
    EditarInstituicaoComponent
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
    MatTooltipModule,
    CpfPipe,
    CnpjPipe,
    NgxMaskDirective
  ],
  providers: [
    VoluntarioService,
    AutorizacaoService,  
    InstituicaoService,
    provideNgxMask()
  ]
})
export class ConfiguracoesModule { }
