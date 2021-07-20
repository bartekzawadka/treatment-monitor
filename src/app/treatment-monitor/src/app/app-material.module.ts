import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {MatDialogModule} from "@angular/material/dialog";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatSelectModule} from "@angular/material/select";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatIconModule} from "@angular/material/icon";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatListModule} from "@angular/material/list";
import {MatInputModule} from "@angular/material/input";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatChipsModule} from "@angular/material/chips";
import {MatMenuModule} from "@angular/material/menu";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatTableModule} from "@angular/material/table";
import {MatCardModule} from "@angular/material/card";
import {MatNativeDateModule} from "@angular/material/core";
import {MatTabsModule} from "@angular/material/tabs";
import {MatTooltipModule} from "@angular/material/tooltip";
import {MatButtonModule} from "@angular/material/button";

const materialComponents = [
  CommonModule,
  MatToolbarModule, MatButtonModule, MatIconModule,
  MatMenuModule, MatGridListModule, MatCardModule, MatFormFieldModule,
  MatInputModule, MatListModule, MatDialogModule, MatCheckboxModule,
  MatProgressSpinnerModule, MatButtonModule, MatSelectModule, MatDatepickerModule,
  MatNativeDateModule, MatTabsModule, MatTableModule, MatChipsModule, MatPaginatorModule,
  MatTooltipModule, MatAutocompleteModule, MatSlideToggleModule
];

@NgModule({
  imports: materialComponents,
  exports: materialComponents
})

export class AppMaterialModule { }
