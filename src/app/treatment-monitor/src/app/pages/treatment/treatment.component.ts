import {Component, OnInit} from '@angular/core';
import {PageBase} from "../page-base";
import {DialogService} from "../../services/dialog.service";
import {Location} from "@angular/common";
import {MatDialog} from "@angular/material/dialog";
import {TreatmentMedicineComponent} from "../treatment-medicine/treatment-medicine.component";
import Treatment from "../../models/treatment";
import Medicine from "../../models/medicine";
import {TreatmentService} from "../../services/treatment.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-treatment',
  templateUrl: './treatment.component.html',
  styleUrls: ['./treatment.component.scss']
})
export class TreatmentComponent extends PageBase implements OnInit {
  public model = new Treatment();

  constructor(protected dialogService: DialogService,
              protected location: Location,
              private router: Router,
              private route: ActivatedRoute,
              private dialog: MatDialog,
              private treatmentService: TreatmentService) {
    super(dialogService, location);


  }

  ngOnInit(): void {
    this.route.params.subscribe(async value => {
      const id = value.id;
      if (id) {
        this.loadData(id);
      }
    });
  }

  loadData(id: string) {
    this.dialogService.callWithLoader(() => this.treatmentService.getById(id))
      .subscribe(value => {
        console.log(value);
        this.model = value;
      })
  }

  addMedicine() {
    const ref = this.dialog.open(TreatmentMedicineComponent, {
      disableClose: true
    });

    ref.afterClosed().subscribe(value => {
      if (value) {
        this.model.medicines.push(value);
      }
    });
  }

  editMedicine(medicine: Medicine) {
    const ref = this.dialog.open(TreatmentMedicineComponent, {
      disableClose: true,
      data: medicine
    });

    ref.afterClosed().subscribe(value => {
      if (value) {
        medicine.id = value.id;
        medicine.startDate = value.startDate;
        medicine.name = value.name;
        medicine.numberOfDays = value.numberOfDays;
      }
    });
  }

  removeMedicine(index: number) {
    this.model.medicines.splice(index, 1);
  }

  canSave() {
    return this.model.name && this.model.startDate;
  }

  cancel() {
    this.goBack();
  }

  save() {
    const action = this.model.id
      ? () => this.treatmentService.updateTreatment(this.model)
      : () => this.treatmentService.addTreatment(this.model);

    this.dialogService.callWithLoader(action)
      .subscribe(
        () => this.router.navigate(['/']),
        error => this.showErrorDialog(error, 'Saving treatment failed'));
  }
}
