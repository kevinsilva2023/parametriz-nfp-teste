import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-certificado',
  standalone: false,
  templateUrl: './certificado.component.html',
  styles: ``
})
export class CertificadoComponent {
  certificado!: any;
  temCertificado = false;

  errors: [] = [];

  constructor(//private voluntarioService: VoluntarioService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    // this.obterPorInsituicao();
  }

  atualizarCertificado() {
    // this.obterPorInsituicao();
  }

  // obterPorInsituicao() {
  //   this.voluntarioService.obterPorInsituicao()
  //     .subscribe({
  //       next: (sucesso: any) => { this.processarSucesso(sucesso); },
  //       error: (falha: any) => { this.processarFalha(falha); }
  //     });
  // }

  processarSucesso(response: any) {
    const temDados = response && Object.keys(response).length > 0;
    if (temDados) {
      this.certificado = response;
    }
    this.temCertificado = temDados;
  }

  limparErros() {
    this.errors = [];
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao consultar voluntario.', 'Erro!');
  }
}
