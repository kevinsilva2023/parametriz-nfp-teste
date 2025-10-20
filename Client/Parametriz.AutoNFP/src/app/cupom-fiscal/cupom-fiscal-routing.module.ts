import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CupomFiscalComponent } from './cupom-fiscal.component';
import { cupomFiscalStatusResolver } from './services/cupom-fiscal-status.resolver';
import { obterUsuariosAtivosResolver } from '../configuracoes/usuarios/services/usuarios-ativos.resolver';

const routes: Routes = [
  {
    path: '',
    component: CupomFiscalComponent,
    resolve: {
      status: cupomFiscalStatusResolver,
      usuariosAtivos: obterUsuariosAtivosResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CupomFiscalRoutingModule { }
