import { Injectable } from '@angular/core';
import {ApiService} from "./api.service";
import {HttpClient} from "@angular/common/http";
import TreatmentListItem from "../models/treatment-list-item";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class TreatmentService extends ApiService{
  protected root = 'treatment';

  constructor(protected http: HttpClient) {
    super(http);
  }

  getTreatmentsList(): Observable<TreatmentListItem[]> {
    return this.get<TreatmentListItem[]>(``);
  }
}
