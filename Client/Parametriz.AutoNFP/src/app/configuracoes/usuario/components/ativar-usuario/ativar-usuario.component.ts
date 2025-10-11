import { Component, Input } from '@angular/core';
import { Usuario } from '../../models/usuario';
import { UsuarioService } from '../../services/usuario.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ativar-usuario',
  standalone: false,
  templateUrl: './ativar-usuario.component.html',
  styles: ``
})
export class AtivarUsuarioComponent {
  @Input() usuario!: Usuario;

  errors: [] = [];

  constructor(private usuarioService: UsuarioService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    console.log(this.usuario);
  }

  confirmarAtivarUsuario() {
    this.usuarioService.ativarUsuario(this.usuario)
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Usuário ativado com sucesso.', 'Sucesso!');

    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível cadastrar o usuário.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close();
  }
}
