import { Component, OnInit } from '@angular/core';
import TreatmentListItem from "../../models/treatment-list-item";
import {TreatmentService} from "../../services/treatment.service";
import {PageBase} from "../page-base";
import {DialogService} from "../../services/dialog.service";
import {Location} from "@angular/common";

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
    protected location: Location) {
    super(dialogService, location);
  }

  ngOnInit(): void {
    console.log('Initializing...');
    this.loadData();
  }

  loadData() {
    console.log('Loading data...');
    this
      .dialogService
      .callWithLoader(() => this.treatmentService.getTreatmentsList())
      .subscribe(value => this.treatments = value, error => this.showErrorDialog(error, 'Failure'));
  }
}
