import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NavbarComponent } from './components/navbar/navbar.component';

import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';
import { CnpjPipe } from '../shared/pipe/cnpj.pipe';
import { PerfilService } from '../perfil/services/perfil.service';



@NgModule({
  declarations: [
    LayoutComponent,
    SidebarComponent,
    NavbarComponent
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    MatBadgeModule,
    MatBadgeModule,
    MatMenuModule,
    CnpjPipe
  ], providers: [
    PerfilService
  ]
})
export class LayoutModule { }
