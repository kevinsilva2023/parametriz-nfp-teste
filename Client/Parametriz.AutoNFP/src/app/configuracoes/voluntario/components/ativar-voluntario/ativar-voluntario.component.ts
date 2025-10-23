import { Component } from '@angular/core';
import { Voluntario } from '../../models/voluntario';
import { VoluntarioService } from '../../services/voluntario.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ativar-voluntario',
  standalone: false,
  templateUrl: './ativar-voluntario.component.html',
  styles: ``
})
export class AtivarVoluntarioComponent {
  voluntario!: Voluntario;

  errors: [] = [];

  constructor(private voluntarioService: VoluntarioService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    console.log(this.voluntario);
  }

  confirmarAtivarVoluntario() {
    this.voluntarioService.ativar(this.voluntario)
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Voluntário ativado com sucesso.', 'Sucesso!');

    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível cadastrar o Voluntário.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close();
  }
}
