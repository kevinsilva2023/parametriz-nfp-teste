import { Injectable } from '@angular/core';
import { Claim } from '../models/claim';
import { LocalStorageUtils } from '../utils/local-storage-utils';

@Injectable()
export class AutorizacaoService {

   voluntarioEstaLogado(): boolean {
    const accessTokenEstaExpirado = LocalStorageUtils.accessTokenEstaExpirado();

    if (accessTokenEstaExpirado)
      return false;
    
    return true;
  }

  voluntarioPossuiClaim(claim: Claim): boolean {
    let voluntario = LocalStorageUtils.obterUsuario();

    if (!claim)
      return true;
    
    if (!voluntario?.claims)
      return false;

    let possuiClaim = voluntario.claims.some((c: Claim) => c.type == claim.type && c.value == claim.value) > 0;
  
    return possuiClaim;
  }

  voluntarioPossuiClaims(claims: Claim[]): boolean {
    let possuiClaim: boolean = false;

    claims.forEach(claim => {
      if (this.voluntarioPossuiClaim(claim))
        possuiClaim = true;
    });

    return possuiClaim;
  }
}
