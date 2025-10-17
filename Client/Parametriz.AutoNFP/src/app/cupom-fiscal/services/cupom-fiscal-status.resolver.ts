import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { CupomFiscalService } from './cupom-fiscal.service';

export const cupomFiscalStatusResolver: ResolveFn<Enumerador[]> = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot): Observable<Enumerador[]> => {
  
  let cupomFiscalService = inject(CupomFiscalService);

  return cupomFiscalService.obterStatus();
};
