import { Injectable } from '@angular/core';
import { catchError, map, Observable, retry } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base.service';
import { ErroTransmissaoLote } from '../models/erro-transmissao-lote';

@Injectable()
export class ErroTransmissaoLoteService extends BaseService {

  obterErrosTransmissaoLote(): Observable<ErroTransmissaoLote[]> {
    return this.httpClient
      .get(`${this.apiUrl}/erros-transmissao-lote`, { headers: super.ObterAuthHeaderJson()} )
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }
}
