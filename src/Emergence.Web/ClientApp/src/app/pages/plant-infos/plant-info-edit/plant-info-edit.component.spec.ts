import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantInfoEditComponent } from './plant-info-edit.component';

describe('PlantInfoEditComponent', () => {
  let component: PlantInfoEditComponent;
  let fixture: ComponentFixture<PlantInfoEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantInfoEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantInfoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
