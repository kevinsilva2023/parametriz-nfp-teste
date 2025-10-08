import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LancarNfpComponent } from './lancar-nfp.component';

const routes: Routes = [{ path: '', component: LancarNfpComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LancarNfpRoutingModule { }
