import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageUtils } from 'src/app/shared/utils/local-storage-utils';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  @Input() tituloPagina: string = 'Página';

  constructor(private router: Router) {
  }

  usuario = {
    nome: 'Kevin Marcos'
  };

}
