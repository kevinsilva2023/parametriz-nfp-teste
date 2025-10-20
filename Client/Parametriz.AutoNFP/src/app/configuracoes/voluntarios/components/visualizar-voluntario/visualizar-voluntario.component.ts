import { Component, Input, input, OnInit, resolveForwardRef } from '@angular/core';
import { Voluntario } from '../../models/voluntario';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ExcluirVoluntarioComponent } from '../excluir-voluntario/excluir-voluntario.component';
import { subscribeOn } from 'rxjs';

@Component({
  selector: 'app-visualizar-voluntario',
  standalone: false,
  templateUrl: './visualizar-voluntario.component.html'
})
export class VisualizarVoluntarioComponent {
  @Input() errors: [] = [];
  @Input() voluntario!: Voluntario | null;

  constructor(private modalService: NgbModal) { }

  limparErros() {
    this.errors = [];
  }

  confirmarRemoverVolunatario() {
    let modalRef = this.modalService.open(ExcluirVoluntarioComponent, { size: 'md', centered: true });
    modalRef.closed
      .subscribe((resultado) => {
        if (resultado) {
          this.voluntario = null;
        }
      });
  }
}
