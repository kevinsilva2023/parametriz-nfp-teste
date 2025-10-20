import { Component, OnInit } from '@angular/core';
import { Voluntario } from './models/voluntario';
import { VoluntarioService } from './services/voluntario.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-voluntario',
  standalone: false,
  templateUrl: './voluntario.component.html',
})

export class VoluntarioComponent implements OnInit {
  voluntario!: Voluntario;
  temVoluntario = false;

  errors: [] = [];

  constructor(private voluntarioService: VoluntarioService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.obterPorInsituicao();
  }

  atualizarVoluntario() {
    this.obterPorInsituicao();
  }

  obterPorInsituicao() {
    this.voluntarioService.obterPorInsituicao()
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      });
  }

  processarSucesso(response: any) {
    const temDados = response && Object.keys(response).length > 0;
    if (temDados) {
      this.voluntario = response;
    }
    this.temVoluntario = temDados;
  }

  limparErros() {
    this.errors = [];
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao consultar voluntario.', 'Erro!');
  }

}
