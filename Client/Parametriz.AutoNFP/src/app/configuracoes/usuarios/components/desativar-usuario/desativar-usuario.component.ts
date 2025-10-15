import { Component, Input, OnInit } from '@angular/core';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from '../../models/usuario';
import { ToastrService } from 'ngx-toastr';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-desativar-usuario',
  standalone: false,
  templateUrl: './desativar-usuario.component.html',
  styles: ``
})
export class DesativarUsuarioComponent implements OnInit {
  usuario!: Usuario;

  errors: [] = [];

  constructor(private usuarioService: UsuarioService,
    private activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    console.log(this.usuario);
  }

  confirmarInativarUsuario() {
    this.usuarioService.desativar(this.usuario)
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Usuário desativado com sucesso.', 'Sucesso!');

    this.fecharModal();
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível desativar o usuário.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

  fecharModal() {
    this.activeModal.close();
  }
}