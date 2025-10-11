import { Injectable } from '@angular/core';
import { Usuario } from '../models/usuario';
import { BaseService } from 'src/app/shared/services/base.service';
import { catchError, map, Observable } from 'rxjs';

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
}
