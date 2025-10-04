import { Injectable } from '@angular/core';
import { IdentidadeService } from 'src/app/identidade/services/identidade.service';
import { Claim } from '../models/claim';
import { LocalStorageUtils } from '../utils/local-storage-utils';

@Injectable()
export class AutorizacaoService {

  constructor(private identidadeService: IdentidadeService) { }

   usuarioEstaLogado(): boolean {
    const accessTokenEstaExpirado = LocalStorageUtils.accessTokenEstaExpirado();

    if (accessTokenEstaExpirado) {
      LocalStorageUtils.limparDadosLocaisUsuario();
      return false;
    }

    return true;
  }

  usuarioPossuiClaim(claim: Claim): boolean {
    let usuario = LocalStorageUtils.obterUsuario();

    if (!claim)
      return true;
    
    if (!usuario?.claims)
      return false;

    let possuiClaim = usuario.claims.some((c: Claim) => c.type == claim.type && c.value == claim.value) > 0;
  
    return possuiClaim;
  }

  usuarioPossuiClaims(claims: Claim[]): boolean {
    let possuiClaim: boolean = false;

    claims.forEach(claim => {
      if (this.usuarioPossuiClaim(claim))
        possuiClaim = true;
    });

    return possuiClaim;
  }
}
