import { Component } from '@angular/core';
import { AutorizacaoService } from '../shared/services/autorizacao.service';
import { Claim } from '../shared/models/claim';

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {}
