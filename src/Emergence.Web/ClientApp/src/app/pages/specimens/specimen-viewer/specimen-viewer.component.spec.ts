import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecimenViewerComponent } from './specimen-viewer.component';

describe('SpecimenViewerComponent', () => {
  let component: SpecimenViewerComponent;
  let fixture: ComponentFixture<SpecimenViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecimenViewerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecimenViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
