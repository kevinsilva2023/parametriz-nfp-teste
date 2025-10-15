import { Component, Input, input, OnInit, resolveForwardRef } from '@angular/core';
import { ConsultarVoluntario } from '../../models/consultar-voluntario';

@Component({
  selector: 'app-visualizar-voluntario',
  standalone: false,
  templateUrl: './visualizar-voluntario.component.html'
})
export class VisualizarVoluntarioComponent {
  @Input() errors: [] = [];
  @Input() voluntario!: ConsultarVoluntario | null;

  limparErros() {
    this.errors = [];
  }

}
