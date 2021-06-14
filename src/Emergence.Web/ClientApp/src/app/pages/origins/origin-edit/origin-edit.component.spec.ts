import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginEditComponent } from './origin-edit.component';

describe('OriginEditComponent', () => {
  let component: OriginEditComponent;
  let fixture: ComponentFixture<OriginEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OriginEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
