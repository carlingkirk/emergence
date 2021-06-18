import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecimenModalComponent } from './specimen-modal.component';

describe('SpecimenModalComponent', () => {
  let component: SpecimenModalComponent;
  let fixture: ComponentFixture<SpecimenModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecimenModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecimenModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
