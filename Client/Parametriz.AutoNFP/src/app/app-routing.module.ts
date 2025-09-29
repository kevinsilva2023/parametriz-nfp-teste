import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { AppComponent } from './app.component';

const routes: Routes = [
  {
    path: '',
    component: AppComponent
  },
  { 
    path: 'login', 
    loadChildren: () => import('./auth/login/login.module')
      .then(m => m.LoginModule) 
  },
  { path: 'cadastro', loadChildren: () => import('./auth/cadastro/cadastro.module').then(m => m.CadastroModule) },
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
