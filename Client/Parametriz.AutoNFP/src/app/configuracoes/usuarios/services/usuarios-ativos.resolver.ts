import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { UsuarioService } from './usuario.service';
import { AutorizacaoService } from 'src/app/shared/services/autorizacao.service';
import { Claim } from 'src/app/shared/models/claim';

export const obterUsuariosAtivosResolver: ResolveFn<ObterUsuarioAtivo[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot): Observable<ObterUsuarioAtivo[]> => {

  let usuarioService = inject(UsuarioService);
  let autorizacaoService = inject(AutorizacaoService);

  let claimAdmin: Claim = { type: 'role', value: 'Administrador' };

  let usuarioEhAdmin = autorizacaoService.usuarioPossuiClaim(claimAdmin);

  if(usuarioEhAdmin) {
    return usuarioService.obterUsuariosAtivos();
  } else {
    return of([]);
  }
};
