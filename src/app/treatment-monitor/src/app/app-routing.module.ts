import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {TreatmentsListComponent} from "./pages/treatments-list/treatments-list.component";
import {TreatmentComponent} from "./pages/treatment/treatment.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'treatments',
    pathMatch: 'full'
  },
  {
    path: 'treatment',
    component: TreatmentComponent
  },
  {
    path: 'treatment/:id',
    component: TreatmentComponent
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
