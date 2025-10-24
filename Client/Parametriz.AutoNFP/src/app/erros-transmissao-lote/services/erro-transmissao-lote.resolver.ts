import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { ErroTransmissaoLote } from '../models/erro-transmissao-lote';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { ErroTransmissaoLoteService } from './erro-transmissao-lote.service';

export const erroTransmissaoLoteResolver: ResolveFn<ErroTransmissaoLote[]> = (
  route: ActivatedRouteSnapshot, 
  state: RouterStateSnapshot): Observable<ErroTransmissaoLote[]> => {

  let erroTransmissaoLoteService = inject(ErroTransmissaoLoteService);

  return erroTransmissaoLoteService.obterErrosTransmissaoLote();
};
