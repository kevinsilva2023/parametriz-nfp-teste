import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { CertificadoService } from '../../services/certificado.service';

@Component({
  selector: 'app-excluir-certificado',
  standalone: false,
  templateUrl: './excluir-certificado.component.html',
  styles: ``
})
export class ExcluirCertificadoComponent {
  errors: [] = [];

  constructor(
    private certifcadoService: CertificadoService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  confirmarExcluirCertificado() {
    this.certifcadoService.excluir()
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Certificado excluído com sucesso.', 'Sucesso!');

    this.activeModal.close(true);
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível excluir o certificado.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close();
  }
}
