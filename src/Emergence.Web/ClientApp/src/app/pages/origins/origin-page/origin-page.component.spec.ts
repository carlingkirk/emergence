import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginPageComponent } from './origin-page.component';

describe('OriginPageComponent', () => {
  let component: OriginPageComponent;
  let fixture: ComponentFixture<OriginPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OriginPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
