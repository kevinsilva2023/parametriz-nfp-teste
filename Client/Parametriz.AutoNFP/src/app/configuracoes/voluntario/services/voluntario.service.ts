import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base.service';
import { CadastrarVoluntario } from '../models/cadastrar-voluntario';
import { ConsultarVoluntario } from '../models/consultar-voluntario';

@Injectable()
export class VoluntarioService extends BaseService {

  cadastrar(voluntario: CadastrarVoluntario): Observable<CadastrarVoluntario> {
    return this.httpClient
      .post(`${this.apiUrl}/voluntarios`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  consultar(): Observable<ConsultarVoluntario> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios`,{ headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }
}

