import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LocalStorageUtils } from '../utils/local-storage-utils';

export const jwtInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>, 
  next: HttpHandlerFn): Observable<HttpEvent<any>> => {

    if (req.url.startsWith(`${environment.apiUrl}/refresh-token`))
      return next(req);

    const accessTokenExpirado = LocalStorageUtils.accessTokenEstaExpirado();
    const refreshTokenExpirado = LocalStorageUtils.refreshTokenEstaExpirado();

    if (accessTokenExpirado && refreshTokenExpirado)
      return next(req);

    return next(req);
};
