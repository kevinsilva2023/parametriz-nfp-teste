import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { PerfilComponent } from '../perfil/perfil.component';

const routes: Routes = [
  { path: '', component: LayoutComponent, children: [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'configuracoes', loadChildren: () => import('../configuracoes/configuracoes.module').then(m => m.ConfiguracoesModule) },
    { path: 'perfil', component: PerfilComponent},
  ] }, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
