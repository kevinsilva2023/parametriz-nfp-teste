import { Component, Input } from '@angular/core';
import { ExcluirCertificadoComponent } from '../excluir-certificado/excluir-certificado.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-visualizar-certificado',
  standalone: false,
  templateUrl: './visualizar-certificado.component.html',
  styles: ``
})
export class VisualizarCertificadoComponent {
  @Input() errors: [] = [];
  @Input() certificado!: any // incluir model | null;

  constructor(private modalService: NgbModal) { }

  limparErros() {
    this.errors = [];
  }

  confirmarRemoverVolunatario() {
    let modalRef = this.modalService.open(ExcluirCertificadoComponent, { size: 'md', centered: true });
    
    modalRef.closed
      .subscribe((resultado) => {
        if (resultado) {
          this.certificado = null;
        }
      });
  }
}

// verificar se vai ter tudo que esta no html