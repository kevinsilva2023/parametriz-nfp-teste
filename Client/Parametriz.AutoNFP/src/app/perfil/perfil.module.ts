import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PerfilRoutingModule } from './perfil-routing.module';
import { PerfilComponent } from './perfil.component';
import { CertificadoComponent } from './certificados/certificado.component';
import { VisualizarCertificadoComponent } from './certificados/components/visualizar-certificado/visualizar-certificado.component';
import { CadastrarCertificadoComponent } from './certificados/components/cadastrar-certificado/cadastrar-certificado.component';
import { ExcluirCertificadoComponent } from './certificados/components/excluir-certificado/excluir-certificado.component';
import { EditarPerfilComponent } from './components/editar-perfil/editar-perfil.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CertificadoService } from './certificados/services/certificado.service';
import { PerfilService } from './services/perfil.service';

import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { CpfPipe } from '../shared/pipe/cpf.pipe';
import { VoluntarioService } from '../configuracoes/voluntarios/services/voluntario.service';

@NgModule({
  declarations: [
    PerfilComponent,
    CertificadoComponent,
    VisualizarCertificadoComponent,
    CadastrarCertificadoComponent,
    ExcluirCertificadoComponent,
    EditarPerfilComponent
  ],
  imports: [
    CommonModule,
    PerfilRoutingModule,
    NgbModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatMenuModule,
    MatTooltipModule,
    NgxMaskDirective,
    CpfPipe
  ], 
  providers: [
    PerfilService,
    CertificadoService,
    VoluntarioService,
    provideNgxMask()
  ]
})
export class PerfilModule { }
