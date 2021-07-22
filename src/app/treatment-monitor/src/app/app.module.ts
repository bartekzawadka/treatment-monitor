import {LOCALE_ID, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {AppMaterialModule} from "./app-material.module";
import {HttpClientModule} from "@angular/common/http";
import {MAT_DATE_LOCALE} from "@angular/material/core";
import {MessageDialogComponent} from "./dialogs/message-dialog/message-dialog.component";
import {LoaderDialogComponent} from "./dialogs/loader-dialog/loader-dialog.component";
import {ConfirmationDialogComponent} from "./dialogs/confirmation-dialog/confirmation-dialog.component";
import { TreatmentsListComponent } from './pages/treatments-list/treatments-list.component';
import { TreatmentComponent } from './pages/treatment/treatment.component';
import { TreatmentMedicineComponent } from './pages/treatment-medicine/treatment-medicine.component';
import '@angular/common/locales/global/pl';

@NgModule({
  declarations: [
    AppComponent,
    LoaderDialogComponent,
    MessageDialogComponent,
    ConfirmationDialogComponent,
    TreatmentsListComponent,
    TreatmentComponent,
    TreatmentMedicineComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    AppMaterialModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'pl-PL'
    },
    {
      provide: LOCALE_ID,
      useValue: 'pl-PL'
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
