import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlantInfosListPageComponent } from './plant-infos-list-page.component';

describe('PlantInfosListPageComponent', () => {
  let component: PlantInfosListPageComponent;
  let fixture: ComponentFixture<PlantInfosListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantInfosListPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantInfosListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
