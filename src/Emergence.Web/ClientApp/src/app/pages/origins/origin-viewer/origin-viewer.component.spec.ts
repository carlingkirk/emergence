import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginViewerComponent } from './origin-viewer.component';

describe('OriginViewerComponent', () => {
  let component: OriginViewerComponent;
  let fixture: ComponentFixture<OriginViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OriginViewerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
