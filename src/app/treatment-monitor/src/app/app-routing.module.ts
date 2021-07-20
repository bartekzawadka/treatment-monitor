import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {TreatmentsListComponent} from "./pages/treatments-list/treatments-list.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'treatments',
    pathMatch: 'full'
  },
  {
    path: 'treatments',
    component: TreatmentsListComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
