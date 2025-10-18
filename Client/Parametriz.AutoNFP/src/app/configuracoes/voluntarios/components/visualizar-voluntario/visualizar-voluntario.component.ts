import { Component, Input, input, OnInit, resolveForwardRef } from '@angular/core';
import { ConsultarVoluntario } from '../../models/consultar-voluntario';
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
  @Input() voluntario!: ConsultarVoluntario;

  constructor(private modalService: NgbModal) { }

  limparErros() {
    this.errors = [];
  }

  confirmarRemoverVolunatario() {
    this.modalService.open(ExcluirVoluntarioComponent, { size: 'md', centered: true });
  }
}
