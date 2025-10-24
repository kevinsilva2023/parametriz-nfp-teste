import { Component, Input, OnInit } from '@angular/core';
import { ExcluirCertificadoComponent } from '../excluir-certificado/excluir-certificado.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Certificado } from '../../models/certificado';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';
import { Voluntario } from 'src/app/configuracoes/voluntarios/models/voluntario';

@Component({
  selector: 'app-visualizar-certificado',
  standalone: false,
  templateUrl: './visualizar-certificado.component.html',
  styles: ``
})
export class VisualizarCertificadoComponent {
  @Input() errors: [] = [];
  @Input() voluntario!: Voluntario | null;

  instituicao!: string;

  constructor(private modalService: NgbModal) { }

  confirmarRemoverVolunatario() {
    let modalRef = this.modalService.open(ExcluirCertificadoComponent, { size: 'md', centered: true });

    modalRef.closed
      .subscribe((resultado) => {
        if (resultado) {
          this.voluntario = null;
        }
      });
  }

  limparErros() {
    this.errors = [];
  }
}