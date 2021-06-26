import { Location } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ActivityService } from 'src/app/service/activity-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { onImgError } from 'src/app/shared/common';
import { Editor } from 'src/app/shared/interface/editor';
import { Activity } from 'src/app/shared/models/activity';
import { ActivityType, Visibility } from 'src/app/shared/models/enums';
import { Photo } from 'src/app/shared/models/photo';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-activity-edit',
  templateUrl: './activity-edit.component.html',
  styleUrls: ['./activity-edit.component.css']
})
export class ActivityEditComponent extends Editor {

  @Input()
  public activity: Activity;
  public searching: boolean;
  public searchFailed: boolean;
  public specimens: Specimen[];
  public selectedSpecimen: Specimen;
  public uploadedPhotos: Photo[];
  public activityTypes: ActivityType[];
  public isNewSpecimen: boolean;
  public updateSpecimen: boolean;

  constructor(
    authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private readonly activityService: ActivityService,
    private router: Router,
    route: ActivatedRoute,
    private location: Location
    ) {
      super(authorizeService, route);
     }

  specimenResultFormatter = (result: Specimen) => result.lifeform.scientificName;
  specimenInputFormatter = (x: Specimen) => x.lifeform.scientificName;

  ngOnInit(): void {
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

    return this.specimenService.findSpecimens(searchRequest).pipe(map(
      (searchResult) => searchResult.results));
  }

  public specimensTypeahead: OperatorFunction<string, readonly Specimen[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchSpecimens(term).pipe((specimen) => specimen )))

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

  public saveActivity(): void {
    this.activity.specimen = this.selectedSpecimen;
    this.activity.photos = this.uploadedPhotos;

    this.activityService.saveActivity(this.activity).subscribe(
      (activity) => this.router.navigate(['/activities/', activity.activityId]),
      (error) => {
        console.log(error);
        this.errorMessage = 'There was an error saving the activity';
      });
  }

  public cancel(): void {
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
