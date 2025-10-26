import { Injectable } from '@angular/core';
import { catchError, map, Observable, Subject } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base.service';
import { Instituicao } from '../models/instituicao';

@Injectable()
export class InstituicaoService extends BaseService {

  public static atualizarNavSubject = new Subject<boolean>();

  obter(): Observable<Instituicao> {
    return this.httpClient
      .get(`${this.apiUrl}/instituicoes`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }

  editar(instituicao: Instituicao): Observable<Instituicao> {
    return this.httpClient
      .put(`${this.apiUrl}/instituicoes`, instituicao, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError)
      )
  }
}
