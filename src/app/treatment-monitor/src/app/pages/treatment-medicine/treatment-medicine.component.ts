import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-treatment-medicine',
  templateUrl: './treatment-medicine.component.html',
  styleUrls: ['./treatment-medicine.component.scss']
})
export class TreatmentMedicineComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<TreatmentMedicineComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }

  onCancel(){
    this.dialogRef.close();
  }
}
