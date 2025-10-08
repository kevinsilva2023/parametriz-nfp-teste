import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LancarNfpRoutingModule } from './lancar-nfp-routing.module';
import { LancarNfpComponent } from './lancar-nfp.component';


@NgModule({
  declarations: [
    LancarNfpComponent
  ],
  imports: [
    CommonModule,
    LancarNfpRoutingModule
  ]
})
export class LancarNfpModule { }
