import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CupomFiscalRoutingModule } from './cupom-fiscal-routing.module';
import { CupomFiscalComponent } from './cupom-fiscal.component';
import { CadastrarCupomFiscalComponent } from './components/cadastrar-cupom-fiscal/cadastrar-cupom-fiscal.component';
import { VisualizarCupomFiscalComponent } from './components/visualizar-cupom-fiscal/visualizar-cupom-fiscal.component';


import { ListarCupomFiscalComponent } from './components/listar-cupom-fiscal/listar-cupom-fiscal.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatSelectModule } from '@angular/material/select';

import { provideMomentDateAdapter } from '@angular/material-moment-adapter';

import { FormsModule } from '@angular/forms';
import { CupomFiscalService } from './services/cupom-fiscal.service';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { CnpjPipe } from '../shared/pipe/cnpj.pipe';


@NgModule({
  declarations: [
    CupomFiscalComponent,
    CadastrarCupomFiscalComponent,
    VisualizarCupomFiscalComponent,
    ListarCupomFiscalComponent
  ],
  imports: [
    CommonModule,
    CupomFiscalRoutingModule,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    MatSelectModule,
    NgbPaginationModule,
    NgbModule,
    CnpjPipe
  ],
  providers: [
    CupomFiscalService,
    provideMomentDateAdapter({
      parse: {
        dateInput: 'MM/YYYY',
      },
      display: {
        dateInput: 'MM/YYYY',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
      },
    }) 
  ]
})
export class CupomFiscalModule { }
