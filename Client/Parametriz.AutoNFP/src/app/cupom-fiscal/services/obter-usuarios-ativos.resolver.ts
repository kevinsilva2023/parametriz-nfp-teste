import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { CupomFiscalService } from './cupom-fiscal.service';

export const obterUsuariosAtivosResolver: ResolveFn<ObterUsuarioAtivo[]> = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot): Observable<ObterUsuarioAtivo[]> => {
  
  let cupomFiscalService = inject(CupomFiscalService);

  return cupomFiscalService.obterUsuariosAtivos();

};
