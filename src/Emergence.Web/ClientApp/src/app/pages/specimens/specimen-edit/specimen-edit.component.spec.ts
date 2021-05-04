import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecimenEditComponent } from './specimen-edit.component';

describe('SpecimenEditComponent', () => {
  let component: SpecimenEditComponent;
  let fixture: ComponentFixture<SpecimenEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecimenEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecimenEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
