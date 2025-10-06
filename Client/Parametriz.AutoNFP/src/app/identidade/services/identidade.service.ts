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
import moment from 'moment';

@Injectable()
export class IdentidadeService extends BaseService {

  login(login: Login): Observable<any> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/login`, login, { headers: super.ObterHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  enviarConfirmarEmail(enviarConfirmarEmail: EnviarConfirmarEmail): Observable<EnviarConfirmarEmail> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/enviar-confirmar-email`, enviarConfirmarEmail, { headers: this.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  confirmarEmail(confirmarEmail: ConfirmarEmail): Observable<ConfirmarEmail> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/confirmar-email`, confirmarEmail, { headers: this.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  enviarDefinirSenha(enviarDefinirSenha: EnviarDefinirSenha): Observable<EnviarDefinirSenha> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/enviar-definir-senha`, enviarDefinirSenha, { headers: this.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(this.serviceError));
  }

  definirSenha(definirSenha: DefinirSenha): Observable<DefinirSenha> {
    return this.httpClient
      .post(`${this.apiUrl}/identidade/definir-senha`, definirSenha, { headers: this.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  utilizarRefreshToken(): Observable<any> {
    let refreshToken = `\"${LocalStorageUtils.obterRefreshToken()?.token}\"`;

    return this.httpClient
      .post(`${this.apiUrl}/identidade/refresh-token`, refreshToken, { headers: this.ObterHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }
}
