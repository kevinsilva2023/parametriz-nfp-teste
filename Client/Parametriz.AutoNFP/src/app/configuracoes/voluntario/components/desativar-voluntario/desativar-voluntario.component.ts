import { Component, OnInit } from '@angular/core';
import { Voluntario } from '../../models/voluntario';
import { VoluntarioService } from '../../services/voluntario.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-desativar-voluntario',
  standalone: false,
  templateUrl: './desativar-voluntario.component.html',
  styles: ``
})
export class DesativarVoluntarioComponent{
  voluntario!: Voluntario;

  errors: [] = [];

  constructor(
    private voluntarioService: VoluntarioService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  confirmarInativarVoluntario() {
    this.voluntarioService.desativar(this.voluntario)
      .subscribe({
        next: () => { this.processarSucesso(); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso() {
    this.limparErros()

    this.toastr.success('Voluntário desativado com sucesso.', 'Sucesso!');

    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível desativar o voluntário.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close();
  }
}