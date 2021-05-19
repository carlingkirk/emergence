import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PhotoService } from 'src/app/service/photo-service';
import { onImgError } from '../../common';
import { Photo } from '../../models/photo';

@Component({
  selector: 'app-view-photos',
  templateUrl: './view-photos.component.html',
  styleUrls: ['./view-photos.component.css']
})
export class ViewPhotosComponent implements OnInit {

  @Input()
  public photos: Photo[];
  @Input()
  public isEditing: boolean;
  @Input()
  public name: string;
  @Output()
  public photosChange = new EventEmitter<Photo[]>();
  constructor(private readonly photoService: PhotoService, private modalService: NgbModal) { }

  ngOnInit(): void {
  }

  public showPhotoModal(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
  }

  public removePhotoAsync(id: number) {
    this.photoService.removePhoto(id).subscribe(() => {
      this.photos = this.photos.filter((photo) => photo.photoId !== id);
      this.photosChange.emit(this.photos);
    });
  }

  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }
}
