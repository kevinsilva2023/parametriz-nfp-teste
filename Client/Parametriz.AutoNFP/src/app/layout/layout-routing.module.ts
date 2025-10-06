import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  { path: '', component: LayoutComponent }, 
  { path: 'lanca-nfp', loadChildren: () => import('../lancar-nfp/lancar-nfp.module').then(m => m.LancarNfpModule) },
  { path: 'dashboard', loadChildren: () => import('../dashboard/dashboard.module').then(m => m.DashboardModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
