import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConfiguracoesComponent } from './configuracoes.component';
import { VoluntarioComponent } from './components/voluntario/voluntario.component';
import { Voluntario2Component } from './components/voluntario2/voluntario2.component';

const routes: Routes = [
    {
    path: '',
    component: ConfiguracoesComponent,
    children: [
      { path: 'voluntario', component: VoluntarioComponent},
      { path: 'voluntario2', component: Voluntario2Component },
      { path: '', redirectTo: 'voluntario', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfiguracoesRoutingModule { }
