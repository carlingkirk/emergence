import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SortableHeadersComponent } from './sortable-headers.component';

describe('SortableHeadersComponent', () => {
  let component: SortableHeadersComponent;
  let fixture: ComponentFixture<SortableHeadersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SortableHeadersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SortableHeadersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
