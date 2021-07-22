import { Injectable } from '@angular/core';
import {ApiService} from "./api.service";
import {HttpClient} from "@angular/common/http";
import TreatmentListItem from "../models/treatment-list-item";
import {Observable} from "rxjs";
import Treatment from "../models/treatment";

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

  getById(id: string): Observable<Treatment> {
    return this.get<Treatment>(`/${id}`);
  }

  addTreatment(treatment: Treatment): Observable<Treatment> {
    return this.http.post<Treatment>(this.getEndpointUrl(''), treatment);
  }

  updateTreatment(treatment: Treatment): Observable<Treatment> {
    return this.http.put<Treatment>(this.getEndpointUrl(`/${treatment.id}`), treatment);
  }

  deleteTreatment(id: string): Observable<any> {
    return this.delete(id);
  }
}
