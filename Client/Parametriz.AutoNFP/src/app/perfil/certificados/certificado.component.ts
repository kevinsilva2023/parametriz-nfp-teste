import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CertificadoService } from './services/certificado.service';
import { VoluntarioService } from 'src/app/configuracoes/voluntario/services/voluntario.service';
import { PerfilService } from '../services/perfil.service';
import { Voluntario } from 'src/app/configuracoes/voluntario/models/voluntario';

@Component({
  selector: 'app-certificado',
  standalone: false,
  templateUrl: './certificado.component.html',
  styles: ``
})
export class CertificadoComponent {
  voluntario!: Voluntario;
  temCertificado = false;

  errors: [] = [];

  constructor(
    private perfilService: PerfilService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.obterPorVoluntario();
  }

  atualizarCertificado() {
    this.obterPorVoluntario();
  }

  obterPorVoluntario() {
    this.perfilService.obter()
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      });
  }

  processarSucesso(voluntario: Voluntario) {
    const temDados = voluntario && Object.keys(voluntario).length > 0;
    if (temDados) {
      this.voluntario = voluntario;
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
