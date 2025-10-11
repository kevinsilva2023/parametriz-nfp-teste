import { Injectable } from '@angular/core';
import { Usuario } from '../models/usuario';
import { BaseService } from 'src/app/shared/services/base.service';
import { catchError, map, Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { StringUtils } from 'src/app/shared/utils/string-utils';

@Injectable()
export class UsuarioService extends BaseService {

  cadastrar(usuario: Usuario): Observable<Usuario> {
    return this.httpClient
      .post(
        `${this.apiUrl}/usuarios`, usuario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      );
  }

  obterPorFiltro(usuarioNome: string, usuarioEmail: string, administrador: number, desativado: number): Observable<Usuario[]> {

    let params = new HttpParams;

    if (!StringUtils.isNullOrEmpty(usuarioNome))
      params = params.append('nome', usuarioNome);

    if (!StringUtils.isNullOrEmpty(usuarioEmail))
      params = params.append('email', usuarioEmail);

    if (administrador !== 2)
      params = params.append('administrador', administrador.toString());

    if (desativado !== 2)
      params = params.append('desativado', desativado.toString());

    return this.httpClient
      .get<Usuario[]>(
        `${this.apiUrl}/usuarios/obter-por-filtros`, { headers: super.ObterAuthHeaderJson(), params })
      .pipe(
        catchError(super.serviceError)
      );
  }

  inativarUsuario(usuario: Usuario): Observable<Usuario> {
    return this.httpClient
      .put(`${this.apiUrl}/usuarios/desativar`, usuario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }
}
