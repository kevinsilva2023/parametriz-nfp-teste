import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConfiguracoesComponent } from './configuracoes.component';

const routes: Routes = [
    {
    path: '',
    component: ConfiguracoesComponent,
    children: [
      { path: 'voluntario', loadChildren: () => import('./voluntario/voluntario.module').then(m => m.VoluntarioModule) },
      { path: '', redirectTo: 'voluntario', pathMatch: 'full' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfiguracoesRoutingModule { }
