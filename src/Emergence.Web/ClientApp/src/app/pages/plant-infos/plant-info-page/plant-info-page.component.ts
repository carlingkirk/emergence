import { Component, Input, OnInit } from '@angular/core';
import { getElementId } from 'src/app/shared/common';
import { Photo } from 'src/app/shared/models/photo';
import { PlantInfo } from 'src/app/shared/models/plant-info';

@Component({
  selector: 'app-plant-info-page',
  templateUrl: './plant-info-page.component.html',
  styleUrls: ['./plant-info-page.component.css']
})
export class PlantInfoPageComponent implements OnInit {

  @Input()
  public plantInfo: PlantInfo;
  public uploadedPhotos: Photo[];
  public commonName: string;
  public scientificName: string;

  constructor() { }

  ngOnInit(): void {
    this.scientificName = this.plantInfo.scientificName ?? this.plantInfo.lifeform.scientificName;
    this.commonName = this.plantInfo.commonName ?? this.plantInfo.lifeform.commonName;
  }

  displayRange(start: string, end: string, unit: string = "") {
    let range = null;
    if (!end)
    {
        range = start + unit;
    } else if (start)
    {
        range = `${start} - ${end + unit}`;
    }

    return range;
  }

  getElementId(element: string, id: string) {
    return getElementId(element, id);
  }
}
