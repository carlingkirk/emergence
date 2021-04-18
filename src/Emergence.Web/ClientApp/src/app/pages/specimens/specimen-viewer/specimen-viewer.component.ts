import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SpecimenService } from 'src/app/service/specimen-service';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-viewer',
  templateUrl: './specimen-viewer.component.html',
  styleUrls: ['./specimen-viewer.component.css']
})
export class SpecimenViewerComponent implements OnInit {
  @Input()
  public id: number;
  public specimen: Specimen;
  public tabs: any = [ 
    { key: 'specimen', value: 'Specimen'},
    { key: 'activities', value: 'Activities'},
    { key: 'plant-infos', value: 'Plant Profiles'}
  ];
  public currentTab: string = 'specimen';
  public isEditing: boolean = false;
  public isOwner: boolean = false;
  constructor(
    private readonly specimenService: SpecimenService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    console.log("Id: " + this.id);
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => 
        this.specimen = specimen
    );
  }

  public switchTab(tab: string) {
    this.currentTab = tab;
  }

  public goBack() {
  }

  public showMessageModal() {
  }

  public removeSpecimen() {
  }
}
