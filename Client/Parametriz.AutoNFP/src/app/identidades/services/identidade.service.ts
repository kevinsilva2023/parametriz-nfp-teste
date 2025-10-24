import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/services/base.service';
import { Login } from '../models/login';
import { catchError, map, Observable } from 'rxjs';
import { EnviarConfirmarEmail } from '../models/enviar-confirmar-email';
import { ConfirmarEmail } from '../models/confirmar-email';
import { EnviarDefinirSenha } from '../models/enviar-definir-senha';
import { DefinirSenha } from '../models/definir-senha';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { CadastrarInstituicao } from '../models/cadastrar-instituicao';
// import { Instituicao } from '../models/cadastrar-instituicao';

@Injectable()
export class IdentidadeService extends BaseService {

  login(login: Login): Observable<any> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/login`, login, { headers: super.ObterHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  registrar(instituicao: CadastrarInstituicao): Observable<any> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/cadastrar-instituicao`, instituicao, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  enviarConfirmarEmail(enviarConfirmarEmail: EnviarConfirmarEmail): Observable<EnviarConfirmarEmail> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/enviar-confirmar-email`, enviarConfirmarEmail, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  confirmarEmail(confirmarEmail: ConfirmarEmail): Observable<ConfirmarEmail> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/confirmar-email`, confirmarEmail, { headers: super.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  enviarDefinirSenha(enviarDefinirSenha: EnviarDefinirSenha): Observable<EnviarDefinirSenha> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/enviar-definir-senha`, enviarDefinirSenha, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(this.serviceError));
  }

  definirSenha(definirSenha: DefinirSenha): Observable<DefinirSenha> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/definir-senha`, definirSenha, { headers: super.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  utilizarRefreshToken(): Observable<any> {
    let refreshToken = `\"${LocalStorageUtils.obterRefreshToken()?.token}\"`;

    return this.httpClient
      .post(`${this.apiUrl}/identidade/refresh-token`, refreshToken, { headers: super.ObterHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }
  
  obterDadosCnpj(cnpj: string): Observable<any> { //alterar tipo do retorno depois
    let url = `https://receitaws.com.br/v1/cnpj/${cnpj}`;

    return this.httpClient.jsonp(url, 'callback')
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      );
  }
}
