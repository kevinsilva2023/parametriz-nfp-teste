import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AutorizacaoService } from './autorizacao.service';
import { inject } from '@angular/core';
import { Claim } from '../models/claim';

export const autorizacaoGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot) => {

    let autorizacaoService = inject(AutorizacaoService);
    let router = inject(Router);

    if (!autorizacaoService.voluntarioEstaLogado())
      router.navigate(['/login'], { queryParams: { returnUrl: state.url }});

    let routeData = route.data[0];

    if (routeData) {
      let claims: Claim[] = routeData['claims'];

      if (claims) {
        if (!autorizacaoService.voluntarioPossuiClaims(claims))
            router.navigate(['/acesso-negado']);
      }
    }
    return true;
};
