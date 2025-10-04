import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { throwError } from 'rxjs';

import { LocalStorageUtils } from '../utils/local-storage-utils';

import { CustomResponse } from './dtos/custom-response';
import { inject } from '@angular/core';
import { environment } from 'src/environments/environment';

export abstract class BaseService {

  protected apiUrl: string = environment.apiUrl;
  protected httpClient: HttpClient = inject(HttpClient);
  
  protected ObterHeaderJson(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  protected ObterAuthHeaderJson(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${LocalStorageUtils.obterAccessToken()}`
    });
  }

  protected extractData(response: any): any {
    return response || {};
  }

  protected serviceError(response: Response | any) {
    let customError: string[] = [];
    let customResponse: CustomResponse = new CustomResponse();

    if (response instanceof HttpErrorResponse) {
      if (response.statusText === "Unknown Error") {
        customError.push("Ocorreu um erro desconhecido");
        customResponse.error.errors.mensagens = customError;
        return throwError(() => customResponse);
      }
    }

    if (response.status === 500) {
      customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
      customResponse.error.errors.mensagens = customError;
      return throwError(() => customResponse);
    }

    console.error(response);
    return throwError(() => response);
  }
}
