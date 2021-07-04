import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { getSpecimenName, getSpecimenScientificName } from 'src/app/shared/common';
import { Viewer } from 'src/app/shared/interface/viewer';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-viewer',
  templateUrl: './specimen-viewer.component.html',
  styleUrls: ['./specimen-viewer.component.css']
})
export class SpecimenViewerComponent extends Viewer {
  @Input()
  public modal: NgbModalRef;
  @Input()
  public id: number;
  @Input()
  public specimen: Specimen;
  public tabs: any = [
    { key: 'specimen', value: 'Specimen'},
    { key: 'activities', value: 'Activities'},
    { key: 'plant-infos', value: 'Plant Profiles'}
  ];
  public currentTab = 'specimen';
  public isAuthenticated: Observable<boolean>;
  public name: string;
  public scientificName: string;

  constructor(
    authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    route: ActivatedRoute,
    private router: Router) {
      super(authorizeService, route);
    }

  ngOnInit(): void {
    super.ngOnInit();

    if (this.id > 0 && !this.specimen) {
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
        this.specimen = specimen;
        this.name = getSpecimenName(specimen);
        this.scientificName = getSpecimenScientificName(specimen);
        return of({});
      });
    } else {
      this.id = this.specimen.specimenId
      this.name = getSpecimenName(this.specimen);
      this.scientificName = getSpecimenScientificName(this.specimen);
    }
  }

  public switchTab(tab: string) {
    this.currentTab = tab;
  }

  public goBack() {
  }

  public showMessageModal() {
  }

  public removeSpecimen() {
    this.specimenService.deleteSpecimen(this.id).subscribe(() => {
      this.router.navigate(['/specimens/list']);
    });
  }

  public edit() {
    this.isEditing = true;
  }
}
