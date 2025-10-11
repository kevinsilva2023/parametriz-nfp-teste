import { Component, Input, OnInit } from '@angular/core';
import { UsuarioService } from '../services/usuario.service';
import { Usuario } from '../models/usuario';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-inativar-usuario',
  standalone: false,
  templateUrl: './inativar-usuario.component.html',
  styles: ``
})
export class InativarUsuarioComponent implements OnInit {
  @Input() usuario!: Usuario;

  errors:[] = [];

  constructor(private usuarioService: UsuarioService,
              private modalService: NgbModal,
              private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    console.log(this.usuario);
  }


  confirmarInativarUsuario() {
    this.usuarioService.inativarUsuario(this.usuario)
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()

    this.toastr.success('Usuário inativado com sucesso.', 'Sucesso!');

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
    this.modalService.dismissAll();
  }
}