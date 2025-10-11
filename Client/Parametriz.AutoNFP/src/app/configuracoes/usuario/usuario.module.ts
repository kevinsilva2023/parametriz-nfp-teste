import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsuarioRoutingModule } from './usuario-routing.module';
import { UsuarioComponent } from './usuario.component';
import { CadastrarUsuarioComponent } from './components/cadastrar-usuario/cadastrar-usuario.component';


import { FormsModule } from "@angular/forms";
import { ReactiveFormsModule } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';


import { AlertModule } from 'ngx-bootstrap/alert';
import { ListarUsuarioComponent } from './components/listar-usuario/listar-usuario.component';
import { InativarUsuarioComponent } from './inativar-usuario/inativar-usuario.component';





@NgModule({
  declarations: [
    UsuarioComponent,
    CadastrarUsuarioComponent,
    ListarUsuarioComponent,
    InativarUsuarioComponent,
  ],
  imports: [
    CommonModule,
    UsuarioRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatInputModule,
    AlertModule,
    MatMenuModule
  ],

})
export class UsuarioModule { }
