import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../../services/usuario.service';
import { debounceTime, Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InativarUsuarioComponent } from '../inativar-usuario/inativar-usuario.component';
import { Usuario } from '../../models/usuario';
import { CadastrarUsuarioComponent } from '../cadastrar-usuario/cadastrar-usuario.component';
import { AtivarUsuarioComponent } from '../ativar-usuario/ativar-usuario.component';

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

  debounceNomeUsuario = new Subject<string>();
  debounceEmailUsuario = new Subject<string>();

  constructor(private usuarioService: UsuarioService,
    private modalService: NgbModal
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

  inativarUsuario(usuario: Usuario) {
    let modalRef = this.modalService.open(InativarUsuarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.usuario = usuario

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

  ativarUsuario(usuario: Usuario) {
    let modalRef = this.modalService.open(AtivarUsuarioComponent, { size: 'lg', centered: true })

    modalRef.componentInstance.usuario = usuario

    modalRef.closed
      .subscribe({
        next: () => this.obterPorFiltro()
      });
  }

}
