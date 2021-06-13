import { Component, Input, OnInit } from '@angular/core';
import { getElementId } from 'src/app/shared/common';
import { ConservationStatus, LocationStatus } from 'src/app/shared/models/enums';
import { Photo } from 'src/app/shared/models/photo';
import { PlantInfo, PlantLocation } from 'src/app/shared/models/plant-info';

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
  public conservationGroups: [{ conservationStatus: ConservationStatus, countryGroups: [{ country: string, states: string[]}] }];
  constructor() { }

  ngOnInit(): void {
    this.scientificName = this.plantInfo.scientificName ?? this.plantInfo.lifeform.scientificName;
    this.commonName = this.plantInfo.commonName ?? this.plantInfo.lifeform.commonName;

    this.initializeGroups();
  }

  initializeGroups() {
    this.plantInfo.locations.forEach(location => {
      if (location.status.toString() === "Native") {
        if (!this.conservationGroups) {
          this.conservationGroups = [{ 
            conservationStatus: location.conservationStatus, 
            countryGroups: [{
              country: location.location.country, 
              states: [ location.location.stateOrProvince ?? "Federal" ] 
            }]
          }];
        } else {
          let group = this.conservationGroups.find(group => group.conservationStatus == location.conservationStatus );
          if (!group) {
              this.conservationGroups.push({ 
                conservationStatus: location.conservationStatus, 
                countryGroups: [{
                  country: location.location.country, 
                  states: [ location.location.stateOrProvince ] 
              }]
            });
          } else {
            var subGroup = group.countryGroups.find(group => group.country == location.location.country);
            if (!subGroup) {
              group.countryGroups.push({ country: location.location.country, states: [location.location.stateOrProvince]});
            } else {
              subGroup.states.push(location.location.stateOrProvince === "" ? "Federal" : location.location.stateOrProvince);
            }
          }
        }
      }
    });
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
