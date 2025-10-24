import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CupomFiscalComponent } from './cupom-fiscal.component';
import { cupomFiscalStatusResolver } from './services/cupom-fiscal-status.resolver';
import { voluntariosAtivosResolver } from '../configuracoes/voluntarios/services/voluntarios-ativos.resolver';
import { errosTransmissaoLoteResolver } from '../erros-transmissao-lote/services/erros-transmissao-lote.resolver';

const routes: Routes = [
  {
    path: '',
    component: CupomFiscalComponent,
    resolve: {
      status: cupomFiscalStatusResolver,
      usuariosAtivos: voluntariosAtivosResolver,
      errosTransmissaoLote: errosTransmissaoLoteResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CupomFiscalRoutingModule { }
