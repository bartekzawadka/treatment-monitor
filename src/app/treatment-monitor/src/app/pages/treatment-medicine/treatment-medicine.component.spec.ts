import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TreatmentMedicineComponent } from './treatment-medicine.component';

describe('TreatmentMedicineComponent', () => {
  let component: TreatmentMedicineComponent;
  let fixture: ComponentFixture<TreatmentMedicineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TreatmentMedicineComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TreatmentMedicineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
