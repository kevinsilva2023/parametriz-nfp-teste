import { Component, OnInit, resolveForwardRef } from '@angular/core';
import { ConsultarVoluntario } from '../models/consultar-voluntario';
import { ToastrService } from 'ngx-toastr';
import { VoluntarioService } from '../services/voluntario.service';

@Component({
  selector: 'app-visualizar-voluntario',
  standalone: false,
  templateUrl: './visualizar-voluntario.component.html'
})
export class VisualizarVoluntarioComponent implements OnInit {
  voluntario: ConsultarVoluntario = {
    nome: '',
    cnpjCpf: {
      numeroInscricao: ''
    },
    requerente: '',
    validoAPartirDe: '',
    validoAte: '',
    emissor: '',
    status: ''
  }

  errors: [] = [];

  constructor(private voluntarioService: VoluntarioService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.consultarVoluntarioCadastrado();
  }
  
  consultarVoluntarioCadastrado() {
    this.voluntarioService.consultar()
    .subscribe({
      next: (sucesso: any) => { this.processarSucesso(sucesso); },
      error: (falha: any) => { this.processarFalha(falha); }
    });
  }
  
  processarSucesso(response: any) {
    this.limparErros();
    this.voluntario = response;
  }

  limparErros() {
    this.errors = [];
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Erro ao consultar voluntario.', 'Erro!');
  }
}
