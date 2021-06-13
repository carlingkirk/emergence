import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PlantInfosListComponent } from './plant-infos-list.component';

describe('PlantInfosListComponent', () => {
  let component: PlantInfosListComponent;
  let fixture: ComponentFixture<PlantInfosListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlantInfosListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlantInfosListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
