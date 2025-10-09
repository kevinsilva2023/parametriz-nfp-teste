import { Component } from '@angular/core';
import { NGNAV_ITEMS } from './models/ngnav-item';

@Component({
  selector: 'app-configuracoes',
  standalone: false,
  templateUrl: './configuracoes.component.html',
  styleUrl: './configuracoes.component.scss'
})
export class ConfiguracoesComponent {
  ngNavItem = NGNAV_ITEMS
}
