import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/services/base.service';
import { Voluntario } from '../models/voluntario';
import { catchError, map, Observable } from 'rxjs';
import { StringUtils } from 'src/app/shared/utils/string-utils';
import { ObterVoluntarioAtivo } from 'src/app/shared/models/obter-voluntario-ativo';
import { HttpParams } from '@angular/common/http';

@Injectable()
export class VoluntarioService extends BaseService {

  cadastrar(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .post(`${this.apiUrl}/voluntarios`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      );
  }

  editar(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  ativar(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios/ativar`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  desativar(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios/desativar`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  obterPorId(voluntarioId: string): Observable<Voluntario> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios/${voluntarioId}`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  obterPorFiltro(voluntarioNome: string, voluntarioEmail: string, administrador: number, desativado: number): Observable<Voluntario[]> {

    let params = new HttpParams;

    if (!StringUtils.isNullOrEmpty(voluntarioNome))
      params = params.append('nome', voluntarioNome);

    if (!StringUtils.isNullOrEmpty(voluntarioEmail))
      params = params.append('email', voluntarioEmail);

    if (administrador !== 2)
      params = params.append('administrador', administrador.toString());

    if (desativado !== 2)
      params = params.append('desativado', desativado.toString());

    return this.httpClient
      .get<Voluntario[]>(
        `${this.apiUrl}/voluntarios/obter-por-filtros`, { headers: super.ObterAuthHeaderJson(), params })
      .pipe(
        catchError(super.serviceError)
      );
  }

  obterVoluntariosAtivos(): Observable<ObterVoluntarioAtivo[]> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios/obter-ativos`, { headers: this.ObterAuthHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }
}
