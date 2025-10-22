import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Usuario } from 'src/app/configuracoes/usuarios/models/usuario';
import { BaseService } from 'src/app/shared/services/base.service';

@Injectable()
export class PerfilService extends BaseService {

  obter(): Observable<Usuario> {
    return this.httpClient
      .get(`${this.apiUrl}/usuarios/perfil`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  editar(): Observable<Usuario> {
    return this.httpClient
      .get(`${this.apiUrl}/usuarios/perfil`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  salvar(usuario: Usuario): Observable<Usuario> {
    return this.httpClient
      .put(`${this.apiUrl}/usuarios/perfil`, usuario, { headers: super.ObterAuthHeaderJson() })
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
