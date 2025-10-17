import { Component, OnInit } from '@angular/core';
import { Enumerador } from 'src/app/shared/models/enumureador';
import { ActivatedRoute, Data } from '@angular/router';
import { CupomFiscalService } from '../../services/cupom-fiscal.service';
import { ObterUsuarioAtivo } from 'src/app/shared/models/obter-usuario-ativo';
import { CupomFiscal, CupomFiscalResponse } from '../../models/cupom-fiscal';

@Component({
  selector: 'app-listar-cupom-fiscal',
  standalone: false,
  templateUrl: './listar-cupom-fiscal.component.html',
  styles: ``
})

export class ListarCupomFiscalComponent implements OnInit {

  status!: Enumerador[];
  usuariosAtivos!: ObterUsuarioAtivo[];

  cuponsFiscaisCadastrados: CupomFiscal[] = [];

  data = new Date();

  filtroCompetencia = this.data;
  filtroUsuario = '';
  filtroStatus = '';

  constructor(private activatedRoute: ActivatedRoute,
    private cupomFiscalService: CupomFiscalService) {
    this.status = this.activatedRoute.snapshot.data['status'];
    this.usuariosAtivos = this.activatedRoute.snapshot.data['usuariosAtivos'];
  }

  ngOnInit(): void {
    this.obterPorFiltro();
  }

  obterPorFiltro() {
    this.cupomFiscalService.obterPorFiltro(this.filtroCompetencia, this.filtroUsuario, this.filtroStatus)
      .subscribe({
        next: (responde: any) => {
          this.cuponsFiscaisCadastrados = responde.cuponsFiscais;
        },
        error: (err) => console.log(err)
      })
  }

}
