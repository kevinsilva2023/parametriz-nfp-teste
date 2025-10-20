import { Component, ViewChild, viewChild } from '@angular/core';
import { ListarCupomFiscalComponent } from './components/listar-cupom-fiscal/listar-cupom-fiscal.component';

@Component({
  selector: 'app-cupom-fiscal',
  standalone: false,
  templateUrl: './cupom-fiscal.component.html',
  styles: ``
})
export class CupomFiscalComponent {
  @ViewChild(ListarCupomFiscalComponent)
  listarCupomFiscalComponent!: ListarCupomFiscalComponent;

  onCupomCadastrado() {
    this.listarCupomFiscalComponent.obterPorFiltro();
  }

}
