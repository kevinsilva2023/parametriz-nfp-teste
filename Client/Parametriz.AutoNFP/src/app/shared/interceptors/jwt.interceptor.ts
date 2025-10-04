import { HttpClient, HttpEvent, HttpHandler, HttpHandlerFn, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { BehaviorSubject, filter, Observable, Subject, switchMap, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LocalStorageUtils } from '../utils/local-storage-utils';
import { Injectable } from '@angular/core';
import { IdentidadeService } from 'src/app/identidade/services/identidade.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  
  private refreshTokenInProgress = false;
  private refreshTokenSubject: Subject<any> = new BehaviorSubject<any>(null);

  constructor(private identidadeService: IdentidadeService) {}
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    if (req.url.startsWith(`${environment.apiUrl}/refresh-token`))
      return next.handle(req);

    const accessTokenExpirado = LocalStorageUtils.accessTokenEstaExpirado();
    const refreshTokenExpirado = LocalStorageUtils.refreshTokenEstaExpirado();

    if (!accessTokenExpirado)
      return next.handle(this.injectAccessToken(req));

    if (refreshTokenExpirado)
      return next.handle(req);

    if (!this.refreshTokenInProgress) {
      this.refreshTokenInProgress = true;
      this.refreshTokenSubject.next(null);
      return this.identidadeService.utilizarRefreshToken()
        .pipe(
          switchMap((response) => {
            LocalStorageUtils.salvarDadosLocaisUsuario(response);
            this.refreshTokenInProgress = false;
            this.refreshTokenSubject.next(response.refreshToken);
            return next.handle(this.injectAccessToken(req));
          }),
        );
    } else {
      return this.refreshTokenSubject
        .pipe(
          filter(result => result !== null),
          take(1),
          switchMap((res) => {
            return next.handle(this.injectAccessToken(req))
          })
        );
    }
  }

  injectAccessToken(request: HttpRequest<any>) {
    const accessToken = LocalStorageUtils.obterAccessToken();
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
    
}
