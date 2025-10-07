import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { LoginComponent } from "./identidade/components/login/login.component";
import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { EmailConfirmadoComponent } from './identidade/components/email-confirmado/email-confirmado.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { DefinirSenhaComponent } from './identidade/components/definir-senha/definir-senha.component';
import { EsqueceuASenhaComponent } from './identidade/components/esqueceu-a-senha/esqueceu-a-senha.component';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';


import { FundoAnimadoComponent } from "src/app/shared/components/fundo-animado/fundo-animado.component";
import { NaoEncontradoComponent } from './components/nao-encontrado/nao-encontrado.component';
import { AcessoNegadoComponent } from './components/acesso-negado/acesso-negado.component';

import { errorInterceptor } from './shared/interceptors/error.interceptor';
import { jwtInterceptor } from './shared/interceptors/jwt.interceptor';
import { IdentidadeService } from './identidade/services/identidade.service';
import { AutorizacaoService } from './shared/services/autorizacao.service';

import { AlertModule } from 'ngx-bootstrap/alert';


@NgModule({
  declarations: [
    AppComponent,
    RegistrarComponent,
    LoginComponent,
    ConfirmarEmailComponent,
    EsqueceuASenhaComponent,
    DefinirSenhaComponent,
    EmailConfirmadoComponent,
    FundoAnimadoComponent,
    NaoEncontradoComponent,
    AcessoNegadoComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatCardModule,
    MatSelectModule,
    AlertModule
  ],
  providers: [
    IdentidadeService,
    AutorizacaoService,
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        jwtInterceptor
      ])),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
