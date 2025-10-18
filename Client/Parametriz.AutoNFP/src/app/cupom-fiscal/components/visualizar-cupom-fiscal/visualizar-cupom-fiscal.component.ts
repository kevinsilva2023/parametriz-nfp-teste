import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-visualizar-cupom-fiscal',
  standalone: false,
  templateUrl: './visualizar-cupom-fiscal.component.html',
  styles: ``
})
export class VisualizarCupomFiscalComponent {
  mensagemErro!: string;

  constructor(private activeModal: NgbActiveModal) { }

  fecharModal() {
    this.activeModal.close();
  }
}
