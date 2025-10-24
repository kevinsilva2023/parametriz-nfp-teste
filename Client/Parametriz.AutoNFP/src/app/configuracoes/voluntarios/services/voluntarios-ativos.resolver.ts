import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { Claim } from 'src/app/shared/models/claim';
import { VoluntarioService } from './voluntario.service';
import { ObterVoluntarioAtivo } from 'src/app/shared/models/obter-voluntario-ativo';

export const voluntariosAtivosResolver: ResolveFn<ObterVoluntarioAtivo[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot): Observable<ObterVoluntarioAtivo[]> => {

  let voluntarioService = inject(VoluntarioService);
  let autorizacaoService = inject(AutorizacaoService);

  let claimAdmin: Claim = { type: 'role', value: 'Administrador' };

  let usuarioEhAdmin = autorizacaoService.voluntarioPossuiClaim(claimAdmin);

  if (usuarioEhAdmin) {
    return voluntarioService.obterVoluntariosAtivos();
  } else {
    return of([]);
  }
};
