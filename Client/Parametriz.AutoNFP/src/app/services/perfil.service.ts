import { Injectable } from '@angular/core';
import { BaseService } from '../shared/services/base.service';
import { Usuario } from '../configuracoes/usuarios/models/usuario';
import { catchError, map, Observable } from 'rxjs';

@Injectable()
export class PerfilService extends BaseService {

  obterNaoAdministrador(): Observable<Usuario> {
    return this.httpClient
      .get(`${this.apiUrl}/usuarios/nao-administrador`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  editar(): Observable<Usuario> {
    return this.httpClient
      .get(`${this.apiUrl}/usuarios/nao-administrador`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  salvarImagem(usuario: Usuario): Observable<Usuario> {
    return this.httpClient
      .put(`${this.apiUrl}/usuarios/perfil`, usuario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }
}
