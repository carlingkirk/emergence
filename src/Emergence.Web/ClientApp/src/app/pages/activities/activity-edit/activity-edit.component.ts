import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { ActivityService } from 'src/app/service/activity-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { getElementId, onImgError } from 'src/app/shared/common';
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
export class ActivityEditComponent implements OnInit {

  @Input()
  public activity: Activity;
  @Input()
  public id: number;
  public visibilities: Visibility[];
  public searching: boolean;
  public searchFailed: boolean;
  public specimens: Specimen[];
  public selectedSpecimen: Specimen;
  public user: IUser;
  public uploadedPhotos: Photo[];
  public activityTypes: ActivityType[];
  public isNewSpecimen: boolean;
  public updateSpecimen: boolean;
  public errorMessage: string;

  constructor(
    private authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private readonly activityService: ActivityService,
    private router: Router,
    private route: ActivatedRoute) { }

  specimenResultFormatter = (result: Specimen) => result.lifeform.scientificName;
  specimenInputFormatter = (x: Specimen) => x.lifeform.scientificName;

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    this.activityTypes = Object.keys(ActivityType).filter(key => !isNaN(Number(key))).map(key => ActivityType[key]);
    
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];
      this.loadActivity();
    });
  }

  loadActivity() {
    if (!this.activity && this.id > 0) {
      this.activityService.getActivity(this.id).subscribe((activity) => {
        this.activity = activity;
        this.selectedSpecimen = activity.specimen;
        this.uploadedPhotos = activity.photos;
      });
    }

    if (this.id == 0) {
      this.activity = new Activity();
      this.activity.createdBy = this.user.userId;
      this.activity.dateCreated = new Date();
      this.activity.photos = [];
      this.activity.visibility = this.visibilities[Visibility["Inherit from profile"]];
    }
  }

  searchSpecimens(searchText: string): Observable<Specimen[]> {
    if (searchText === '') {
      return of([]);
    }

    let searchRequest: SearchRequest = {
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
        : this.searchSpecimens(term).pipe((specimen) => specimen )));

  public showAutoSpecimen() {
    return this.activity.activityId == 0 && !this.isNewSpecimen &&
      (this.activity.activityType == ActivityType["Plant in ground"] ||
      this.activity.activityType == ActivityType.Germinate ||
      this.activity.activityType == ActivityType.Stratify ||
      this.activity.activityType == ActivityType["Collect seeds"])
  }

  public populateActivityName() {
    if (!this.activity.name) {
      var name = this.selectedSpecimen?.lifeform?.scientificName ?? this.selectedSpecimen.name ?? "";
      if (this.activity.activityType != ActivityType.Custom){
          this.activity.name = this.activity.activityType + ": " + name;
      } else {
        this.activity.name = this.activity.customActivityType + ": " + name;
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
        this.errorMessage = "There was an error saving the activity";
      });
  }

  public cancel(): void {
    if (this.activity.activityId) {
      this.router.navigate(['/activities/', this.activity.activityId]);
    } else {
      this.router.navigate([".."]);
    }
  }
  
  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }

   photosChanged(photos: Photo[]) {
    this.uploadedPhotos = photos;
  }

  getElementId(element: string, id: string) {
    return getElementId(element, id);
  }
}
