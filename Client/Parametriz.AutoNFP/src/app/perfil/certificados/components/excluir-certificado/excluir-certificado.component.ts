import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-excluir-certificado',
  standalone: false,
  templateUrl: './excluir-certificado.component.html',
  styles: ``
})
export class ExcluirCertificadoComponent {
  errors: [] = [];

  constructor(
    //private voluntarioService: VoluntarioService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  confirmarExcluirCertificado() {
    // this.voluntarioService.excluir()
    //   .subscribe({
    //     next: (sucesso: any) => { this.processarSucesso(sucesso); },
    //     error: (falha: any) => { this.processarFalha(falha); }
    //   })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Voluntario excluído com sucesso.', 'Sucesso!');

    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível excluir o voluntario.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close(true);
  }
}
