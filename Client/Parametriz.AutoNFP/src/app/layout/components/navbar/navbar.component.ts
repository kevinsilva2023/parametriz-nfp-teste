import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  @Input() tituloPagina: string = 'Página';

  usuario = {
    nome: 'Kevin Marcos'
  };

  logout() {}
}
