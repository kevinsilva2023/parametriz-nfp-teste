import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { PerfilComponent } from '../perfil/perfil.component';

const routes: Routes = [
  { path: '', component: LayoutComponent, children: [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'configuracoes', loadChildren: () => import('../configuracoes/configuracoes.module').then(m => m.ConfiguracoesModule) },
    { path: 'perfil', component: PerfilComponent},
    { path: 'cupom-fiscal', loadChildren: () => import('../cupom-fiscal/cupom-fiscal.module').then(m => m.CupomFiscalModule) }, 
  ] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
