import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { CadastrarVoluntario } from 'src/app/perfil/certificados/models/cadastrar-certificado';
import { Voluntario } from 'src/app/perfil/certificados/models/certificado';
import { BaseService } from 'src/app/shared/services/base.service';

@Injectable()
export class CertificadoService extends BaseService {

  // cadastrar tipo cadastrar certificado
  cadastrar(voluntario: CadastrarVoluntario): Observable<CadastrarVoluntario> {
    return this.httpClient
      .post(`${this.apiUrl}/voluntarios`, voluntario, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  // excluir tipo certificado
  excluir(): Observable<Voluntario> {
    return this.httpClient
      .delete(`${this.apiUrl}/voluntarios`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }

  // tipo certificado
  obterPorInsituicao(): Observable<Voluntario> {
    return this.httpClient
      .get(`${this.apiUrl}/voluntarios`, { headers: super.ObterAuthHeaderJson() })
      .pipe(
        map(super.extractData),
        catchError(super.serviceError));
  }
}
