import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Voluntario } from 'src/app/configuracoes/voluntario/models/voluntario';
import { BaseService } from 'src/app/shared/services/base.service';

@Injectable()
export class PerfilService extends BaseService {

  obter(): Observable<Voluntario> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios/perfil`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  editar(): Observable<Voluntario> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios/perfil`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  salvar(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios/perfil`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  salvarImagem(voluntario: Voluntario): Observable<Voluntario> {
    return this.httpClient
      .put(`${this.apiUrl}/voluntarios/perfil`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }
}
