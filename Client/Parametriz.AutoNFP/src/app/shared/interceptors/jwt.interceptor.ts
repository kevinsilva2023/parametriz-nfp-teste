import { HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { LocalStorageUtils } from '../utils/local-storage-utils';
import { JwtSupport } from './jwt-support';
import { inject } from '@angular/core';
import { IdentidadeService } from 'src/app/identidades/services/identidade.service';
import { filter, switchMap, take } from 'rxjs';
import { AutorizacaoService } from '../services/autorizacao.service';

export const jwtInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>, 
  next: HttpHandlerFn) => {
  
    let identidadeService = inject(IdentidadeService);
    let autorizacaoService = inject(AutorizacaoService);

    if (!autorizacaoService.voluntarioEstaLogado())
      return next(req);

    // if (req.url.startsWith(`${environment.apiUrl}/refresh-token`))
    //   return next(req);

    const accessTokenExpirado = LocalStorageUtils.accessTokenEstaExpirado();
    const refreshTokenExpirado = LocalStorageUtils.refreshTokenEstaExpirado();

    if (!accessTokenExpirado)
      return next(JwtSupport.injectAccessToken(req));

    if (refreshTokenExpirado)
      return next(req);

    if (!JwtSupport.refreshTokenInProgress) {
      JwtSupport.refreshTokenInProgress = true;
      JwtSupport.refreshTokenSubject.next(null);
      return identidadeService.utilizarRefreshToken()
        .pipe(
          switchMap((response) => {
            LocalStorageUtils.salvarDadosLocaisUsuario(response);
            JwtSupport.refreshTokenInProgress = false;
            JwtSupport.refreshTokenSubject.next(response.refreshToken);
            return next(JwtSupport.injectAccessToken(req));
          })
        );
    } else {
      return JwtSupport.refreshTokenSubject
        .pipe(
          filter(result => result !== null),
          take(1),
          switchMap((res) => {
            return next(JwtSupport.injectAccessToken(req))
          })
        );
    }
};
