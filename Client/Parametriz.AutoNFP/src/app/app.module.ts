import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http';

import { LoginComponent } from "./identidade/components/login/login.component";
import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { EmailConfirmadoComponent } from './identidade/components/email-confirmado/email-confirmado.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { DefinirNovaSenhaComponent } from './identidade/components/definir-nova-senha/definir-nova-senha.component';
import { EsqueceuASenhaComponent } from './identidade/components/esqueceu-a-senha/esqueceu-a-senha.component';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';

import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule  } from '@angular/forms';

import { FundoAnimadoComponent } from "src/app/shared/components/fundo-animado/fundo-animado.component";


@NgModule({
  declarations: [
    AppComponent,
    RegistrarComponent,
    LoginComponent,
    ConfirmarEmailComponent,
    EsqueceuASenhaComponent,
    DefinirNovaSenhaComponent,
    EmailConfirmadoComponent

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
    FundoAnimadoComponent
],
  providers: [
    provideHttpClient(),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
