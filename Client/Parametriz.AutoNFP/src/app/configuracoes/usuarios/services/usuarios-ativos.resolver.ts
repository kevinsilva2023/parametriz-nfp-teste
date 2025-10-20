import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { UsuarioService } from './usuario.service';

export const obterUsuariosAtivosResolver: ResolveFn<ObterUsuarioAtivo[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot): Observable<ObterUsuarioAtivo[]> => {

  let usuarioService = inject(UsuarioService);

  return usuarioService.obterUsuariosAtivos();

};
