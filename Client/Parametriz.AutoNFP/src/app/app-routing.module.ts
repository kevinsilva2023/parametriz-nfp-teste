import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';

import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { LoginComponent } from './identidade/components/login/login.component';
import { EsqueceuASenhaComponent } from './identidade/components/esqueceu-a-senha/esqueceu-a-senha.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { DefinirSenhaComponent } from './identidade/components/definir-senha/definir-senha.component';
import { EmailConfirmadoComponent } from './identidade/components/email-confirmado/email-confirmado.component';
import { AcessoNegadoComponent } from './shared/components/acesso-negado/acesso-negado.component';
import { NaoEncontradoComponent } from './shared/components/nao-encontrado/nao-encontrado.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarComponent
  },
  { path: 'confirmar-email', component: ConfirmarEmailComponent },
  { path: 'definir-senha', component: DefinirSenhaComponent },
  { path: 'email-confirmado', component: EmailConfirmadoComponent },
  { path: 'esqueceu-a-senha', component: EsqueceuASenhaComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registrar', component: RegistrarComponent },
  { path: 'layout', loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule) },
  { path: 'acesso-negado', component: AcessoNegadoComponent },
  { path: 'nao-encontrado', component: NaoEncontradoComponent },
  { path: '**', component: NaoEncontradoComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
