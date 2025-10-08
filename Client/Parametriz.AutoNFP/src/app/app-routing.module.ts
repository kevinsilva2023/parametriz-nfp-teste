import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';

import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { LoginComponent } from './identidade/components/login/login.component';
import { EsqueceuASenhaComponent } from './identidade/components/esqueceu-a-senha/esqueceu-a-senha.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { ConfirmarEmailEnviadoComponent } from './identidade/components/confirmar-email-enviado/confirmar-email-enviado.component';
import { DefinirSenhaComponent } from './identidade/components/definir-senha/definir-senha.component';
import { DefinirSenhaEnviadoComponent } from './identidade/components/definir-senha-enviado/definir-senha-enviado.component';
import { AcessoNegadoComponent } from './components/acesso-negado/acesso-negado.component';
import { NaoEncontradoComponent } from './components/nao-encontrado/nao-encontrado.component';
import { autorizacaoGuard } from './shared/services/autorizacao.guard';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./layout/layout.module')
      .then(m => m.LayoutModule),
    canActivate: [autorizacaoGuard],
  },
  { path: 'confirmar-email', component: ConfirmarEmailComponent},
  { path: 'confirmar-email-enviado', component: ConfirmarEmailEnviadoComponent },
  { path: 'definir-senha', component: DefinirSenhaComponent },
  { path: 'definir-senha-enviado', component: DefinirSenhaEnviadoComponent },
  { path: 'esqueceu-a-senha', component: EsqueceuASenhaComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registrar', component: RegistrarComponent },
  { path: 'acesso-negado', component: AcessoNegadoComponent },
  { path: 'nao-encontrado', component: NaoEncontradoComponent },
  { path: '**', component: NaoEncontradoComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
