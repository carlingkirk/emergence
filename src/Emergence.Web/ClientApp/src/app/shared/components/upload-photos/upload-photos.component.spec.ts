import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadPhotosComponent } from './upload-photos.component';

describe('UploadPhotosComponent', () => {
  let component: UploadPhotosComponent;
  let fixture: ComponentFixture<UploadPhotosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadPhotosComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadPhotosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
