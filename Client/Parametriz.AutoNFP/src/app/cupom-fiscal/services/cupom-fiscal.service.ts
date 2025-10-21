import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { BaseService } from 'src/app/shared/services/base.service';
import { DataUtils } from 'src/app/shared/utils/data-utils';
import { CupomFiscalPaginacao } from '../models/cupom-fiscal';
import { HttpParams } from '@angular/common/http';
import { StringUtils } from 'src/app/shared/utils/string-utils';
import { CadastrarCupomFiscal } from '../models/cadastrar-cupom-fiscal';

@Injectable()
export class CupomFiscalService extends BaseService {
  processar(qrCode: CadastrarCupomFiscal): Observable<CadastrarCupomFiscal> {
    return this.httpClient
      .post(`${this.apiUrl}/cupons-fiscais`, qrCode, { headers: this.ObterAuthHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  obterStatus(): Observable<Enumerador[]> {
    return this.httpClient
      .get(`${this.apiUrl}/cupons-fiscais/status`, { headers: this.ObterAuthHeaderJson() })
      .pipe(
        map(this.extractData),
        catchError(this.serviceError));
  }

  obterPorFiltro(competencia: Date, cadastradoPorId: string, status: string, pagina: number, registroPorPagina: number): Observable<CupomFiscalPaginacao[]> {
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
      .get<CupomFiscalPaginacao[]>(
        `${this.apiUrl}/cupons-fiscais`, { headers: super.ObterAuthHeaderJson(), params })
      .pipe(
        catchError(super.serviceError)
      );
  }

  obterPorUsuario(competencia: Date, status: string, pagina: number, registroPorPagina: number): Observable<CupomFiscalPaginacao[]> {
    let params = new HttpParams;
    let competenciaFormatada = DataUtils.formatarParaParametro(competencia);

    if (!StringUtils.isNullOrEmpty(competenciaFormatada))
      params = params.append('competencia', competenciaFormatada);

    params = params.append('status', status);
    params = params.append('pagina', pagina);
    params = params.append('registrosPorPagina', registroPorPagina);

    return this.httpClient
      .get<CupomFiscalPaginacao[]>(
        `${this.apiUrl}/cupons-fiscais/obter-por-usuario`, { headers: super.ObterAuthHeaderJson(), params })
      .pipe(
        catchError(super.serviceError)
      );
  }

}
