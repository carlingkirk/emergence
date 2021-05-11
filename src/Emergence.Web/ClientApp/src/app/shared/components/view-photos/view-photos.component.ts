import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PhotoService } from 'src/app/service/photo-service';
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
  @Output()
  public outPhotos = new EventEmitter<Photo[]>();
  constructor(private readonly photoService: PhotoService) { }

  ngOnInit(): void {
  }

  public showPhotoModal() {

  }

  public removePhotoAsync(id: number) {
    this.photoService.removePhoto(id).subscribe(() => {
      this.photos = this.photos.filter((photo) => photo.photoId !== id);
      this.outPhotos.emit(this.photos);
    });
  }

  onImgError(event, photo: Photo) {
    event.onerror = null;
    event.srcset = '';
    event.src = photo.originalUri;
    event.target.src = photo.originalUri;
    event.target.srcset = '';
   }
}
