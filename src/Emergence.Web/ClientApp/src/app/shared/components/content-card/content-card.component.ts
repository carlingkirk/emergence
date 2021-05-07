import { Component, Input, OnInit } from '@angular/core';
import { Photo } from '../../models/photo';
import * as seedrandom from 'seedrandom';
import { Specimen } from '../../models/specimen';

@Component({
  selector: 'content-card',
  templateUrl: './content-card.component.html',
  styleUrls: ['./content-card.component.css']
})
export class ContentCardComponent implements OnInit {
  @Input()
  public compactView: boolean = false;
  @Input()
  public photos: Photo[];
  @Input()
  public name: string;
  @Input()
  public colorSearch: string;
  public color: string;
  public mainPhoto: Photo;
  
  constructor() { }

  private colors = [
    { name: "black", color: "#333333" },
    { name: "gray", color: "#ADAFA8" },
    { name: "red", color: "#E24441" },
    { name: "orange", color: "#E8AD2E" },
    { name: "yellow", color: "#EFEF6E" },
    { name: "green", color: "#94C973" },
    { name: "blue", color: "#5959FF" },
    { name: "purple", color: "#963AFF" },
    { name: "pink", color: "#FF77D6" },
    { name: "teal", color: "#8EFFD5" },
    { name: "olive", color: "#3D550C" }
  ];

  ngOnInit(): void {
    this.color = this.getRandomColor(this.colorSearch);

    if (this.photos) {
      this.mainPhoto = this.photos.length > 0 ? this.photos[0] : null;
    }
  }

  getRandomColor (search: string): string
  {
      let randomValue = 1;
      if (this.colorSearch) {
        var random = seedrandom(search);
        randomValue = Math.abs(random.int32() % this.colors.length);
      } else {
        var random = seedrandom();
        randomValue = Math.abs(random.int32() % this.colors.length);
      }

      var colorPick = this.colors[randomValue];

      if (search) {
        this.colors.forEach((color) => {
            if (search.toLocaleLowerCase().indexOf(color.name) >= 0) {
                return color.color;
            }
        });
        return colorPick.color;
      } else {
        return colorPick.color;
      }
  }

  onImgError(event) {
    event.onerror = null;
    event.srcset = '';
    event.src = this.mainPhoto.originalUri;
    event.target.src = this.mainPhoto.originalUri;
   }

   showPhotoModal() {

   }
}
