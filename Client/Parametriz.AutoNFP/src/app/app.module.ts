import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClient, HttpClientJsonpModule, provideHttpClient, withInterceptors, withJsonpSupport } from '@angular/common/http';

import { LoginComponent } from "./identidade/components/login/login.component";
import { RegistrarComponent } from './identidade/components/registrar/registrar.component';
import { ConfirmarEmailComponent } from './identidade/components/confirmar-email/confirmar-email.component';
import { ConfirmarEmailEnviadoComponent } from './identidade/components/confirmar-email-enviado/confirmar-email-enviado.component';
import { DefinirSenhaComponent } from './identidade/components/definir-senha/definir-senha.component';
import { DefinirSenhaEnviadoComponent } from './identidade/components/definir-senha-enviado/definir-senha-enviado.component';
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

import { ToastrModule } from 'ngx-toastr';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NgbModalModule, NgbModalConfig, NgbAlertModule, NgbAlertConfig } from '@ng-bootstrap/ng-bootstrap';
import { PerfilComponent } from './perfil/perfil.component';
import { PerfilService } from './services/perfil.service';
import { CnpjPipe } from './shared/pipe/cnpj.pipe';


@NgModule({
  declarations: [
    AppComponent,
    RegistrarComponent,
    LoginComponent,
    ConfirmarEmailEnviadoComponent,
    EsqueceuASenhaComponent,
    DefinirSenhaComponent,
    DefinirSenhaEnviadoComponent,
    ConfirmarEmailComponent,
    FundoAnimadoComponent,
    NaoEncontradoComponent,
    AcessoNegadoComponent,
    PerfilComponent,
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
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      progressBar: true,
      progressAnimation: 'increasing',
    }),
    NgbModalModule,
    NgbAlertModule,
    CnpjPipe,
  ],
  providers: [
    IdentidadeService,
    AutorizacaoService,
    PerfilService,
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        jwtInterceptor
      ]),
      withJsonpSupport(),
    ),
    {
      provide: NgbModalConfig,
      useFactory: () => {
        const config = new NgbModalConfig();
        config.centered = true;
        // config.backdrop = 'static';
        config.keyboard = false;
        return config;
      }
    },
    {
      provide: NgbAlertConfig,
      useFactory: () => {
        const config = new NgbAlertConfig();
        config.dismissible = true; 
        config.animation = true;
        return config;
      },
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
