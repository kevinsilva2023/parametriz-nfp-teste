import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/shared/services/base.service';
import { Voluntario } from '../models/voluntario';
import { catchError, map, Observable } from 'rxjs';

@Injectable()
export class VoluntarioService extends BaseService {

  ativar(usuario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios/ativar`, usuario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }
}
