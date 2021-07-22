import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import Medicine from "../../models/medicine";

@Component({
  selector: 'app-treatment-medicine',
  templateUrl: './treatment-medicine.component.html',
  styleUrls: ['./treatment-medicine.component.scss']
})
export class TreatmentMedicineComponent implements OnInit {
  public model = new Medicine();

  constructor(public dialogRef: MatDialogRef<TreatmentMedicineComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Medicine) { }

  ngOnInit(): void {
    if (this.data){
      this.model.name = this.data.name;
      this.model.numberOfDays = this.data.numberOfDays;
      this.model.startDate = this.data.startDate;
      this.model.id = this.data.id;
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  canSave() {
    return this.model.name && this.model.numberOfDays > 0 && this.model.startDate;
  }
}
