import { Component, Input, OnInit } from '@angular/core';
import { PhotoService } from 'src/app/service/photo-service';
import { PhotoType } from '../../models/enums';
import { Photo } from '../../models/photo';

@Component({
  selector: 'app-upload-photos',
  templateUrl: './upload-photos.component.html',
  styleUrls: ['./upload-photos.component.css']
})
export class UploadPhotosComponent implements OnInit {

  @Input()
  public photos: Photo[];
  @Input()
  public type: PhotoType;
  public fileData: File = null;
  public externalUrl: string;

  constructor(private readonly photoService: PhotoService) { }

  ngOnInit(): void {
  }

  uploadPhoto(event) {
      let file = event.target.files[0];

      this.photoService.uploadPhoto(this.type, file)
        .subscribe(
          (photo: Photo) => this.photos.push(photo), 
          (error) => console.log(error));
  }

  addExternalPhoto() {
    this.photoService.addExternalPhoto(this.type, this.externalUrl).subscribe((photo: Photo) => this.photos.push(photo));
  }
}
