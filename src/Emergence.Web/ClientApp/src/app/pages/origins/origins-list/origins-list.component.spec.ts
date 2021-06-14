import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginsListComponent } from './origins-list.component';

describe('OriginsListComponent', () => {
  let component: OriginsListComponent;
  let fixture: ComponentFixture<OriginsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OriginsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
