import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { CupomFiscalService } from './cupom-fiscal.service';
import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { Claim } from 'src/app/shared/models/claim';

export const cupomFiscalStatusResolver: ResolveFn<Enumerador[]> = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot): Observable<Enumerador[]> => {

  let cupomFiscalService = inject(CupomFiscalService);
  let autorizacaoService = inject(AutorizacaoService);

  let claimAdmin: Claim = { type: 'role', value: 'Administrador' };

  let usuarioEhAdmin = autorizacaoService.usuarioPossuiClaim(claimAdmin);

  if (usuarioEhAdmin) {
    return cupomFiscalService.obterStatus();
  } else {
    return of([]);
  }
};
