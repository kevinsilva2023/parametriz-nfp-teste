import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { LocalStorageUtils } from '../utils/local-storage-utils';
import { inject } from '@angular/core';


export const errorInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>, 
  next: HttpHandlerFn): Observable<HttpEvent<any>> => {
    
    let router: Router = inject(Router);
       
    return next(req)
      .pipe(
        catchError(err => {
          if (err instanceof HttpErrorResponse) {
              // Não autenticado
              if (err.status === 401) {
                LocalStorageUtils.limparDadosLocaisUsuario();
                router.navigate(['/login'], { queryParams: { returnUrl: router.url } });
              }
              // Não autorizado
              if (err.status === 403) {
                router.navigate(['/acesso-negado']);
              }
              // Não encontrado - Rotear pelo ** na rota principal
              // if (err.status === 404) {
              //     router.navigate(['/nao-encontrado']);
              // }
          }
          return throwError(() => err);
        })
      );
};
