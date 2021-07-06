import { Location } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { combineLatest, concat, forkJoin, merge, Observable, of, OperatorFunction } from 'rxjs';
import { combineAll, concatMap, debounceTime, distinctUntilChanged, map, mapTo, mergeMap, reduce, switchMap, tap, withLatestFrom } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ActivityService } from 'src/app/service/activity-service';
import { LifeformService } from 'src/app/service/lifeform-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { onImgError } from 'src/app/shared/common';
import { Editor } from 'src/app/shared/interface/editor';
import { Activity } from 'src/app/shared/models/activity';
import { ActivityType, Visibility } from 'src/app/shared/models/enums';
import { Lifeform } from 'src/app/shared/models/lifeform';
import { Photo } from 'src/app/shared/models/photo';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { newSpecimen, Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-activity-edit',
  templateUrl: './activity-edit.component.html',
  styleUrls: ['./activity-edit.component.css']
})
export class ActivityEditComponent extends Editor {

  @Input()
  public activity: Activity;
  @Input()
  public id: number;
  @Input()
  public specimenId: number;
  public searchText: string;
  public searching: boolean;
  public searchFailed: boolean;
  public specimens: Specimen[];
  public selectedSpecimen: Specimen;
  public uploadedPhotos: Photo[];
  public activityTypes: ActivityType[];
  public isNewSpecimen: boolean;
  public updateSpecimen: boolean;
  public editingSpecimen: boolean;

  constructor(
    authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private readonly activityService: ActivityService,
    private readonly lifeformService: LifeformService,
    private router: Router,
    route: ActivatedRoute,
    private location: Location,
    private modalService: NgbModal
    ) {
      super(authorizeService, route);
     }

  specimenResultFormatter = (result: Specimen) => result.lifeform.scientificName;
  specimenInputFormatter = (x: Specimen) => x.name;

  ngOnInit(): void {
    super.ngOnInit();
    
    this.activityTypes = Object.keys(ActivityType).filter(key => !isNaN(Number(key))).map(key => ActivityType[key]);

    this.loadActivity();
  }

  loadActivity() {
    if (!this.activity && this.id > 0) {
      this.activityService.getActivity(this.id).subscribe((activity) => {
        this.activity = activity;
        this.selectedSpecimen = activity.specimen;
        this.uploadedPhotos = activity.photos;
        return of({});
      });
    }

    if (this.id == 0) {
      this.activity = new Activity();
      this.activity.createdBy = this.userId;
      this.activity.dateCreated = new Date();
      this.activity.photos = [];
      this.activity.visibility = this.visibilities[Visibility['Inherit from profile']];
    }
    
    if (!this.specimenId) {
      this.specimenId = this.route.snapshot.queryParams['specimenId'];
    }

    if (this.specimenId) {
      this.specimenService.getSpecimen(this.specimenId).subscribe((specimen) => {
        this.selectedSpecimen = specimen;
        return of({});
      });
    } else {
      this.editingSpecimen = true;
    }
  }

  searchSpecimens(searchText: string): Observable<Specimen[]> {
    if (searchText === '') {
      return of([]);
    }

    const searchRequest: SearchRequest = {
      filters: null,
      searchText: searchText,
      take: 12,
      skip: 0,
      useNGrams: false
    };

    return this.specimenService.findSpecimens(searchRequest).pipe(
      map((searchResult) => searchResult.results.map((specimen) => specimen as Specimen)));
  }

  searchLifeforms(searchText: string): Observable<Lifeform[]> {
    if (searchText === '') {
      return of([]);
    }

    const searchRequest: SearchRequest = {
      filters: null,
      searchText: searchText,
      take: 3,
      skip: 0,
      useNGrams: false
    };

    return this.lifeformService.findLifeforms(searchRequest).pipe(map(
      (searchResult) => searchResult.results));
  }

  searchSpecimensAndLifeforms(searchText: string): Observable<Specimen[]> {
    this.searchText = searchText;
    let specimens = combineLatest([
      this.searchSpecimens(searchText),
      this.searchLifeforms(searchText)
        .pipe(map((searchResult) => searchResult.map((lifeform) => newSpecimen(this.userId, lifeform))))]
    ).pipe(map(([specimens, lifeforms]) => specimens.concat(lifeforms)));

    return specimens;
  }

  public specimensTypeahead: OperatorFunction<string, readonly Specimen[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      concatMap(term => term.length < 2 ? []
        : this.searchSpecimensAndLifeforms(term).pipe((specimen) => specimen )))

  public showAutoSpecimen() {
    return this.activity.activityId === 0 && !this.isNewSpecimen &&
      (this.activity.activityType === ActivityType['Plant in ground'] ||
      this.activity.activityType === ActivityType.Germinate ||
      this.activity.activityType === ActivityType.Stratify ||
      this.activity.activityType === ActivityType['Collect seeds']);
  }

  public populateActivityName() {
    if (!this.activity.name) {
      const name = this.selectedSpecimen?.lifeform?.scientificName ?? this.selectedSpecimen.name ?? '';
      if (this.activity.activityType !== ActivityType.Custom) {
          this.activity.name = this.activity.activityType + ': ' + name;
      } else {
        this.activity.name = this.activity.customActivityType + ': ' + name;
      }
    }
  }

  saveActivity(): void {
    this.activity.specimen = this.selectedSpecimen;
    this.activity.photos = this.uploadedPhotos;

    this.activityService.saveActivity(this.activity).subscribe(
      (activity) => this.router.navigate(['/activities/', activity.activityId]),
      (error) => {
        console.log(error);
        this.errorMessage = 'There was an error saving the activity';
      });
  }

  showSpecimenModal(content, lifeform) {
    if (lifeform) {
      this.selectedSpecimen = newSpecimen(this.userId, lifeform);
    }
    
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title', size: 'lg'})
      .result.then((specimen) => {
      this.selectedSpecimen = specimen;
      this.editingSpecimen = false;
    });
  }

  editSpecimen() {
    this.editingSpecimen = true;
  }

  cancelEditSpecimen(clear: boolean) {
    if (clear) {
      this.selectedSpecimen = null;
    }
    this.editingSpecimen = false;
  }

  cancel(): void {
    if (this.activity.activityId) {
      this.router.navigate(['/activities/', this.activity.activityId]);
    } else {
      this.location.back();
    }
  }

  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }

   photosChanged(photos: Photo[]) {
    this.uploadedPhotos = photos;
  }
}
