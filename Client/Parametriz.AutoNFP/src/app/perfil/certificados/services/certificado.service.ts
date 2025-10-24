import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base.service';
import { CadastrarCertificado } from '../models/cadastrar-certificado';
import { Certificado } from '../models/certificado';

@Injectable()
export class CertificadoService extends BaseService {

  cadastrar(certificado: CadastrarCertificado): Observable<CadastrarCertificado> {
    return this.httpClient
      .post(`${this.apiUrl}/certificados`, certificado, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  excluir(): Observable<Certificado> {
    return this.httpClient
      .delete(`${this.apiUrl}/certificados`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }
}
