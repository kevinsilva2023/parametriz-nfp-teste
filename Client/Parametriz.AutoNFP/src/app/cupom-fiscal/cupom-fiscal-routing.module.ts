import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CupomFiscalComponent } from './cupom-fiscal.component';
import { cupomFiscalStatusResolver } from './services/cupom-fiscal-status.resolver';
import { obterVoluntariosAtivosResolver } from '../configuracoes/voluntario/services/voluntarios-ativos.resolver';

const routes: Routes = [
  {
    path: '',
    component: CupomFiscalComponent,
    resolve: {
      status: cupomFiscalStatusResolver,
      usuariosAtivos: obterVoluntariosAtivosResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CupomFiscalRoutingModule { }
