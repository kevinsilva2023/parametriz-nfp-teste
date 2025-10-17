import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { BaseService } from 'src/app/shared/services/base.service';
import { DataUtils } from 'src/app/shared/utils/data-utils';
import { CupomFiscalResponse } from '../models/cupom-fiscal';
import { HttpParams } from '@angular/common/http';
import { StringUtils } from 'src/app/shared/utils/string-utils';

@Injectable()
export class CupomFiscalService extends BaseService {

  obterStatus(): Observable<Enumerador[]> {
    return this.httpClient
      .get(`${this.apiUrl}/cupons-fiscais/status`, { headers: this.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  obterUsuariosAtivos(): Observable<ObterUsuarioAtivo[]> {
    return this.httpClient
      .get(`${this.apiUrl}/usuarios/obter-ativos`, { headers: this.ObterHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  obterPorFiltro(competencia: Date, cadastradoPorId: string, status: string, pagina: number, registroPorPagina: number): Observable<CupomFiscalResponse[]> {
    let params = new HttpParams;
    let competenciaFormatada = DataUtils.formatarParaParametro(competencia);

    if (!StringUtils.isNullOrEmpty(competenciaFormatada))
      params = params.append('competencia', competenciaFormatada);

    if (!StringUtils.isNullOrEmpty(cadastradoPorId))
      params = params.append('cadastradoPorId', cadastradoPorId);
    
    params = params.append('status', status);
    params = params.append('pagina', pagina);
    params = params.append('registrosPorPagina', registroPorPagina);

    return this.httpClient
      .get<CupomFiscalResponse[]>(
        `${this.apiUrl}/cupons-fiscais`, { headers: super.ObterAuthHeaderJson(), params })
      .pipe(
        catchError(super.serviceError)
      );
  }
}
