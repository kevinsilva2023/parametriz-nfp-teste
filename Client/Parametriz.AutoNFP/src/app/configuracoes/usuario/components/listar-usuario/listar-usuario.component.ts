import { Component, OnInit } from '@angular/core';
import { UsuarioCadastrado } from '../../models/usuario-cadastrado';
import { UsuarioService } from '../../services/usuario.service';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-listar-usuario',
  standalone: false,
  templateUrl: './listar-usuario.component.html',
  styles: `
    .sticky-top { z-index: 999 !important; }
  `
})
export class ListarUsuarioComponent implements OnInit {

  usuariosCadastrados!: UsuarioCadastrado[];

  filtroNomeUsuario = '';
  filtroEmailUsuairo = '';
  filtroAcesso = 2;
  filtroDesativado = 0;

  debounceNomeUsuario = new Subject<string>();
  debounceEmailUsuario = new Subject<string>();

  constructor(private usuarioService: UsuarioService) { }

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

  obterPorFiltro() {
    this.usuarioService.obterPorFiltro(this.filtroNomeUsuario, this.filtroEmailUsuairo,
      this.filtroAcesso, this.filtroDesativado)
      .subscribe({
        next: (usuariosCadastrados: UsuarioCadastrado[]) => {
          this.usuariosCadastrados = usuariosCadastrados
        }
      })
  }


  getInputValue(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }

}
