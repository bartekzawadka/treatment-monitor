import { Component, OnInit } from '@angular/core';
import TreatmentListItem from "../../models/treatment-list-item";
import {TreatmentService} from "../../services/treatment.service";
import {PageBase} from "../page-base";
import {DialogService} from "../../services/dialog.service";
import {Location} from "@angular/common";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-treatments-list',
  templateUrl: './treatments-list.component.html',
  styleUrls: ['./treatments-list.component.scss']
})
export class TreatmentsListComponent extends PageBase implements OnInit {
  public treatments: TreatmentListItem[] = [];

  constructor(
    private treatmentService: TreatmentService,
    protected dialogService: DialogService,
    protected location: Location,
    private dialog: MatDialog) {
    super(dialogService, location);
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this
      .dialogService
      .callWithLoader(() => this.treatmentService.getTreatmentsList())
      .subscribe(value => this.treatments = value, error => this.showErrorDialog(error, 'Failure'));
  }

  deleteTreatment(treatment: TreatmentListItem) {
    this.dialogService.showConfirmation(
      'Deleting treatment',
      'Are you sure you want to delete this treatment?',
      result => {
        if(result){
          this.dialogService.callWithLoader(() => this.treatmentService.deleteTreatment(treatment.id))
            .subscribe(
              () => this.loadData(),
              error => this.showErrorDialog(error, 'Deleting treatment failed'));
        }
      });
  }
}
