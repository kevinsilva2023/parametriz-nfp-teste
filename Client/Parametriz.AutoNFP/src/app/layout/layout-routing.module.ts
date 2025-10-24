import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  {
    path: '', component: LayoutComponent, children: [
      { path: '', redirectTo: 'cupom-fiscal', pathMatch: 'full' },
      {
        path: 'configuracoes',
        loadChildren: () => import('../configuracoes/configuracoes.module')
          .then(m => m.ConfiguracoesModule),
        data: { titulo: 'Configurações' }
      },
      { 
        path: 'perfil', 
        loadChildren: () => import('../perfil/perfil.module')
        .then(m => m.PerfilModule), 
        data: { titulo: 'Perfil Usuário'}
      },
      {
        path: 'cupom-fiscal',
        loadChildren: () => import('../cupons-fiscais/cupom-fiscal.module')
          .then(m => m.CupomFiscalModule),
        data: { titulo: 'Cupom Fiscal' }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
