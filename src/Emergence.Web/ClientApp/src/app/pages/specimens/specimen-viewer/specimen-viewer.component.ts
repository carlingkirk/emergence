import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Observable, of, Subscription } from 'rxjs';
import { mergeMap, take } from 'rxjs/operators';
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
  public getSpecimenSub: Subscription;
  public getUserSub: Subscription;
  public tabs: any = [
    { key: 'specimen', value: 'Specimen'},
    { key: 'activities', value: 'Activities'},
    { key: 'plant-infos', value: 'Plant Profiles'}
  ];
  public currentTab = 'specimen';
  public isEditing = false;
  public isOwner = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
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

    if (!this.specimen) {
      this.getUserSub = this.authorizeService.getUser().subscribe(
        (user) => {
          this.user = user;
          this.user.userId = user['sub'];
          this.getSpecimenSub = this.specimenService.getSpecimen(this.id).subscribe(
            (specimen) => {
              this.specimen = specimen;
              this.isOwner = this.specimen.createdBy === this.user.userId;
              this.name = getSpecimenName(specimen);
              this.scientificName = getSpecimenScientificName(specimen);
              this.specimenLoaded.emit(this.specimen);
          });
      });
  }

    // this.authorizeService.getUser().subscribe((user) => {
    //   this.user = user;
    //   this.user.userId = user['sub'];
    //   this.specimen$ = this.specimenService.getSpecimen(this.id);
    //   this.specimen$.subscribe(
    //     (specimen) => {
    //       this.specimen = specimen;
    //       this.isOwner = this.specimen.createdBy == this.user.userId;
    //       this.name = getSpecimenName(specimen);
    //       this.scientificName = getSpecimenScientificName(specimen);
    //       this.specimenLoaded.emit(this.specimen);
    //       return of({});
    //     },
    //     (error) => console.log(error),
    //     () => { console.log("getSpecimen complete!") });
    //     return of({});
    // });

    // forkJoin([
    //   this.authorizeService.getUser(),
    //   this.specimenService.getSpecimen(this.id)
    // ]).subscribe(([user, specimen]) => {
    //     this.user = user;
    //     this.user.userId = this.user['sub'];
    //     this.specimen = specimen;
    //     this.isOwner = this.specimen.createdBy == this.user.userId;
    //     this.name = getSpecimenName(this.specimen);
    //     this.scientificName = getSpecimenScientificName(this.specimen);
    //     this.specimenLoaded.emit(this.specimen);
    //   },
    //   error => console.log(error),
    //   () => console.log("getSpecimen complete"));
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

  ngOnDestroy() {
    this.getSpecimenSub.unsubscribe();
    this.getUserSub.unsubscribe();
  }
}
