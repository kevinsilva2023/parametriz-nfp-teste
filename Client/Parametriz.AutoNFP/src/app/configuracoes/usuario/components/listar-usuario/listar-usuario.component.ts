import { Component, OnInit } from '@angular/core';
import { UsuarioCadastrado } from '../../models/usuario-cadastrado';
import { UsuarioService } from '../../services/usuario.service';

@Component({
  selector: 'app-listar-usuario',
  standalone: false,
  templateUrl: './listar-usuario.component.html',
  styles: ``
})
export class ListarUsuarioComponent implements OnInit {

  usuariosCadastrados!: UsuarioCadastrado[];

  filtroNomeUsuario = '';
  filtroEmailUsuairo = '';
  filtroAcesso = 2;
  filtroDesativado = 0;
  
  constructor(private usuarioService: UsuarioService) {}

  ngOnInit(): void {
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
}
