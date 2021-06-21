import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginModalComponent } from './origin-modal.component';

describe('OriginModalComponent', () => {
  let component: OriginModalComponent;
  let fixture: ComponentFixture<OriginModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OriginModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
