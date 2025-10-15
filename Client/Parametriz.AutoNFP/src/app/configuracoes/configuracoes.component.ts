import { Component, OnInit } from '@angular/core';
import { Claim } from '../shared/models/claim';
import { AutorizacaoService } from '../shared/services/autorizacao.service';

@Component({
  selector: 'app-configuracoes',
  standalone: false,
  templateUrl: './configuracoes.component.html',
  styleUrl: './configuracoes.component.scss'
})
export class ConfiguracoesComponent  {
  activeTab = 'voluntario';
}
