import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../../services/usuario.service';
import { debounceTime, Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DesativarUsuarioComponent } from '../desativar-usuario/desativar-usuario.component';
import { Usuario } from '../../models/usuario';
import { CadastrarUsuarioComponent } from '../cadastrar-usuario/cadastrar-usuario.component';
import { AtivarUsuarioComponent } from '../ativar-usuario/ativar-usuario.component';
import { EditarUsuarioComponent } from '../editar-usuario/editar-usuario.component';
import { IdentidadeService } from 'src/app/identidade/services/identidade.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-listar-usuario',
  standalone: false,
  templateUrl: './listar-usuario.component.html',
  styles: `
    .sticky-top { z-index: 999 !important; }
  `
})
export class ListarUsuarioComponent implements OnInit {

  usuariosCadastrados!: Usuario[];

  filtroNomeUsuario = '';
  filtroEmailUsuairo = '';
  filtroAcesso = 2;
  filtroDesativado = 0;

  errors: [] = [];

  debounceNomeUsuario = new Subject<string>();
  debounceEmailUsuario = new Subject<string>();

  constructor(private usuarioService: UsuarioService,
    private modalService: NgbModal,
    private indentidadeService: IdentidadeService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.debounceNomeUsuario
      .pipe(debounceTime(500))
      .subscribe({
        next: (nomeUsuario: string) => {
          this.filtroNomeUsuario = nomeUsuario;
          this.obterPorFiltro();
        }
      });

    this.debounceEmailUsuario
      .pipe(debounceTime(500))
      .subscribe({
        next: (emailUsuario: string) => {
          this.filtroEmailUsuairo = emailUsuario;
          this.obterPorFiltro();
        }
      })
    this.obterPorFiltro();
  }

  alterarFiltroAcesso(event: number) {
    this.filtroAcesso = event;
    this.obterPorFiltro();
  }

  alterarFiltroDesativado(event: number) {
    this.filtroDesativado = event;
    this.obterPorFiltro();
  }

  obterPorFiltro() {
    this.usuarioService.obterPorFiltro(this.filtroNomeUsuario, this.filtroEmailUsuairo,
      this.filtroAcesso, this.filtroDesativado)
      .subscribe({
        next: (usuariosCadastrados: Usuario[]) => {
          this.usuariosCadastrados = usuariosCadastrados
        }
      })
  }

  getInputValue(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }

  cadastrar() {
    let modalRef = this.modalService.open(CadastrarUsuarioComponent, { size: 'lg', centered: false });
    let teste = 'kevin'

    modalRef.componentInstance.instituicao = teste

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  desativar(usuario: Usuario) {
    let modalRef = this.modalService.open(DesativarUsuarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.usuario = usuario;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  ativar(usuario: Usuario) {
    let modalRef = this.modalService.open(AtivarUsuarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.usuario = usuario;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  editar(usuarioID: string) {
    let modalRef = this.modalService.open(EditarUsuarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.usuarioId = usuarioID;

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  enviarConfirmarEmail(usuario: Usuario) {
    this.indentidadeService.enviarConfirmarEmail({
      usuarioId: usuario.id,
      definirSenha: false,
    })
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  enviarDefinirSenha(usuario: Usuario) {
    this.indentidadeService.enviarDefinirSenha({ email: usuario.email.conta })
      .subscribe({
        next: (sucesso: any) => { this.processarSucesso(sucesso); },
        error: (falha: any) => { this.processarFalha(falha); }
      })
  }

  processarSucesso(response: any) {
    this.limparErros()
    this.toastr.success('Email enviado.', 'Sucesso!');
  }

  processarFalha(fail: any) {
    this.errors = fail?.error?.errors?.mensagens;
    this.toastr.error('Não foi possível enviar o email.', 'Erro');
  }

  limparErros() {
    this.errors = [];
  }

}
