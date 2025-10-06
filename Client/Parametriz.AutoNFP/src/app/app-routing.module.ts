import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';

import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { LoginComponent } from './identidade/components/login/login.component';
import { EsqueceuASenhaComponent } from './identidade/components/esqueceu-a-senha/esqueceu-a-senha.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { DefinirNovaSenhaComponent } from './identidade/components/definir-nova-senha/definir-nova-senha.component';
import { EmailConfirmadoComponent } from './identidade/components/email-confirmado/email-confirmado.component';

const routes: Routes = [
  {
    path: '',
    component: RegistrarComponent
  },
  { path: 'confirmar-email', component: ConfirmarEmailComponent },
  { path: 'definir-nova-senha', component: DefinirNovaSenhaComponent},
  { path: 'email-confirmado', component: EmailConfirmadoComponent },
  { path: 'esqueceu-a-senha', component: EsqueceuASenhaComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registrar', component: RegistrarComponent },
  // { paht: 'acesso-negado', component: AccessDeniedComponent },
  // { path: 'nao-encontrado': component: NotFoundComponent },
  // { path: '**', component: NotFoundComponent }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
