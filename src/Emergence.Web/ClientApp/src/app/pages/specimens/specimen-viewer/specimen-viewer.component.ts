import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of, Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { getSpecimenName, getSpecimenScientificName } from 'src/app/shared/common';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-viewer',
  templateUrl: './specimen-viewer.component.html',
  styleUrls: ['./specimen-viewer.component.css']
})
export class SpecimenViewerComponent implements OnInit, OnDestroy {
  @Input()
  public id: number;
  @Input()
  public modal: NgbModalRef;
  @Output()
  public specimenLoaded = new EventEmitter<Specimen>();
  public specimen: Specimen;
  public tabs: any = [
    { key: 'specimen', value: 'Specimen'},
    { key: 'activities', value: 'Activities'},
    { key: 'plant-infos', value: 'Plant Profiles'}
  ];
  public currentTab = 'specimen';
  public isEditing = false;
  public isAuthenticated: Observable<boolean>;
  public userId: string;
  public getUserSub: Subscription;
  public isOwner: boolean;
  public name: string;
  public scientificName: string;

  constructor(
    private authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    if (!this.id) {
      this.id = this.route.snapshot.params['id'];
    }

    this.getUserSub = this.authorizeService.getUserId().pipe(tap(u => this.userId = u)).subscribe();

    this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
      this.specimen = specimen;
      this.name = getSpecimenName(specimen);
      this.scientificName = getSpecimenScientificName(specimen);
      this.specimenLoaded.emit(this.specimen);
      return of({});
    });
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

  public editSpecimen() {
    this.isEditing = true;
  }

  ngOnDestroy(): void {
    this.getUserSub.unsubscribe();
  }
}
