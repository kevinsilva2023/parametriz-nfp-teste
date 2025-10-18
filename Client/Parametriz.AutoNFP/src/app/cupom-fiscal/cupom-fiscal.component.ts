import { Component, ViewChild, viewChild } from '@angular/core';
import { ListarUsuarioComponent } from '../configuracoes/usuarios/components/listar-usuario/listar-usuario.component';

@Component({
  selector: 'app-cupom-fiscal',
  standalone: false,
  templateUrl: './cupom-fiscal.component.html',
  styles: ``
})
export class CupomFiscalComponent {
  @ViewChild(ListarUsuarioComponent)
  listarCupomFiscalComponent!: ListarUsuarioComponent;

  onCupomCadastrado() {
    this.listarCupomFiscalComponent.obterPorFiltro();
  }

}
