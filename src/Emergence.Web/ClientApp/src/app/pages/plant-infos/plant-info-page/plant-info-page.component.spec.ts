import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantInfoPageComponent } from './plant-info-page.component';

describe('PlantInfoPageComponent', () => {
  let component: PlantInfoPageComponent;
  let fixture: ComponentFixture<PlantInfoPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantInfoPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantInfoPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
