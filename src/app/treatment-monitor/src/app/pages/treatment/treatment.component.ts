import { Component, OnInit } from '@angular/core';
import {PageBase} from "../page-base";
import {DialogService} from "../../services/dialog.service";
import {Location} from "@angular/common";
import {MatDialog} from "@angular/material/dialog";
import {TreatmentMedicineComponent} from "../treatment-medicine/treatment-medicine.component";

@Component({
  selector: 'app-treatment',
  templateUrl: './treatment.component.html',
  styleUrls: ['./treatment.component.scss']
})
export class TreatmentComponent extends PageBase implements OnInit {

  constructor(protected dialogService: DialogService,
              protected location: Location,
              private dialog: MatDialog) {
    super(dialogService, location);
  }

  ngOnInit(): void {
  }

  addMedicine() {
    const ref = this.dialog.open(TreatmentMedicineComponent, {
      disableClose: true
    });

    ref.afterClosed().subscribe(value => {

    });
  }
}
