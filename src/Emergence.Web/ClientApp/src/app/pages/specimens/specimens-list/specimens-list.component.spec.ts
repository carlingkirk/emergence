import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecimensListComponent } from './specimens-list.component';

describe('SpecimensListComponent', () => {
  let component: SpecimensListComponent;
  let fixture: ComponentFixture<SpecimensListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecimensListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecimensListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
