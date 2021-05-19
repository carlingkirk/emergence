import { Component, Input, OnInit } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { onImgError } from '../../common';
import { PhotoType } from '../../models/enums';
import { Photo } from '../../models/photo';

@Component({
  selector: 'app-photo-modal',
  templateUrl: './photo-modal.component.html',
  styleUrls: ['./photo-modal.component.css']
})
export class PhotoModalComponent implements OnInit {

  @Input()
  public photo: Photo;
  @Input()
  public name: string;
  @Input()
  public modal: NgbModalRef;
  type: string;
  constructor() { }

  ngOnInit(): void {
    this.type = PhotoType[this.photo.type];
  }

  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }
}
