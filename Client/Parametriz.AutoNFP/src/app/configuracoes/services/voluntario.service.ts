import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base.service';

@Injectable()
export class VoluntarioService extends BaseService {

    enviarCerficiadoDigital(certificadoDigital: string): Observable<any> {
      return this.httpClient
        .post(`${this.apiUrl}/identidade/cadastrar-voluntario`, certificadoDigital, { headers: super.ObterAuthHeaderJson() })
        .pipe(
          map(super.extractData),
          catchError(super.serviceError));
    }
}
