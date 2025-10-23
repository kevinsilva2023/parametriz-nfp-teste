import { Component, Input, OnInit } from '@angular/core';
import { ExcluirCertificadoComponent } from '../excluir-certificado/excluir-certificado.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Certificado } from '../../models/certificado';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-visualizar-certificado',
  standalone: false,
  templateUrl: './visualizar-certificado.component.html',
  styles: ``
})
export class VisualizarCertificadoComponent implements OnInit {
  @Input() errors: [] = [];
  @Input() certificado!: Certificado | null;

  instituicao!: string;

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
    this.obterInsituicao();
  }

  obterInsituicao() {
    var result = LocalStorageUtils.obterUsuario();

    this.instituicao = `${result.instituicao.razaoSocial} - ${result.instituicao.cnpj}`
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

  limparErros() {
    this.errors = [];
  }
}