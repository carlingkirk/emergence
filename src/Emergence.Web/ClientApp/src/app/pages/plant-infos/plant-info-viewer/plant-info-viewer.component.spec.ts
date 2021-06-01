import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantInfoViewerComponent } from './plant-info-viewer.component';

describe('PlantInfoViewerComponent', () => {
  let component: PlantInfoViewerComponent;
  let fixture: ComponentFixture<PlantInfoViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantInfoViewerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantInfoViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
