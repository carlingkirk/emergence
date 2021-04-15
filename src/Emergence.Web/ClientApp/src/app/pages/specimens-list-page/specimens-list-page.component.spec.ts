import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecimensListPageComponent } from './specimens-list-page.component';

describe('SpecimensListPageComponent', () => {
  let component: SpecimensListPageComponent;
  let fixture: ComponentFixture<SpecimensListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SpecimensListPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecimensListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
