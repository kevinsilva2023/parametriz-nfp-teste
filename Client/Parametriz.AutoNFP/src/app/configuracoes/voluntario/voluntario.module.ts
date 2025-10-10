import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VoluntarioRoutingModule } from './voluntario-routing.module';
import { VoluntarioComponent } from './voluntario.component';

import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { CadastrarVoluntarioComponent } from './cadastrar-voluntario/cadastrar-voluntario.component';
import { VisualizarVoluntarioComponent } from './visualizar-voluntario/visualizar-voluntario.component';


@NgModule({
  declarations: [
    VoluntarioComponent,
    CadastrarVoluntarioComponent,
    VisualizarVoluntarioComponent
  ],
  imports: [
    CommonModule,
    VoluntarioRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
]
})
export class VoluntarioModule { }
