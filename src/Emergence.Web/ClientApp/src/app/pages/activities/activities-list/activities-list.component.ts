import { Component, Input, OnInit } from '@angular/core';
import { ActivityService } from 'src/app/service/activity-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { Activity } from 'src/app/shared/models/activity';
import { ActivityType } from 'src/app/shared/models/enums';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-activities-list',
  templateUrl: './activities-list.component.html',
  styleUrls: ['./activities-list.component.css']
})
export class ActivitiesListComponent extends Listable implements OnInit, IListable {

  @Input()
  public showSearch: boolean = true;
  @Input()
  public upcoming: boolean;
  @Input()
  public contactsOnly: boolean;
  @Input()
  public specimen: Specimen;
  public activities: Activity[];

  constructor(
    private readonly activityService: ActivityService,
    private readonly storageService: StorageService
    ) {
    super();
  }

  public columns: Column[] = [
    { name: 'Name', value: 'Name'},
    { name: 'Scientific Name', value: 'ScientificName'},
    { name: 'Activity Type', value: 'ActivityType'},
    { name: 'Date Occured', value: 'DateOccured'},
    { name: 'Date Scheduled', value: 'DateScheduled'}
  ];

  ngOnInit(): void {
    let searchType = this.contactsOnly ? "-contacts" : this.upcoming ? "-upcoming" : "";
    if (!this.specimen && !this.contactsOnly && !this.upcoming) {
      this.storageService.getItem("activity-search" + searchType).then((searchRequest) => {
        if (!searchRequest) {
          this.resetSearch();
        } else {
          this.searchRequest = searchRequest;
        }
        this.loadActivities();
      });
    } else {
      this.searchRequest = {
        filters: null,
        take: 12,
        skip: 0,
        useNGrams: false,
        sortDirection: 0,
        sortBy: null,
        contactsOnly: this.contactsOnly
      };
      if (this.specimen) {
        this.loadSpecimenActivities();
      } else if (this.upcoming) {
        this.loadScheduledActivities();
      }
    }
  }

  getColorName(activity: Activity) {
    return activity.specimen?.lifeform?.commonName ?? activity.name
  }

  showScientificName(activity: Activity) {
    return activity.specimen?.lifeform?.scientificName && !activity.name.includes(activity.specimen?.lifeform?.scientificName);
  }

  getActivityType(activity: Activity) {
    var activityType = activity.activityType == ActivityType.Custom ? activity.customActivityType : activity.activityType.toString();
    return activity.name.includes(activityType) ? null : activityType;
  }

  getPhotos(activity: Activity) {
    if (activity.photos) {
      return activity.photos;
    } else {
      return activity.specimenPhotos ?? [];
    }
  }

  loadActivities() {
    this.storageService.setItem("activity-search", this.searchRequest).then((result) => {
      this.activityService.findActivities(this.searchRequest).subscribe(
        (searchResult) => {
          this.searchResult = searchResult;
          this.activities = searchResult.results;
          this.totalCount = searchResult.count;

          this.searchRequest.filters = searchResult.filters;
        }
      );
    });
  }

  loadSpecimenActivities() {
    this.activityService.findSpecimenActivities(this.searchRequest, this.specimen.specimenId).subscribe(
      (searchResult) => {
        this.searchResult = searchResult;
        this.activities = searchResult.results;
        this.totalCount = searchResult.count;

        this.searchRequest.filters = searchResult.filters;
      }
    );
  }

  loadScheduledActivities() {
    this.activityService.findScheduledActivities(this.searchRequest, new Date()).subscribe(
      (searchResult) => {
        this.searchResult = searchResult;
        this.activities = searchResult.results;
        this.totalCount = searchResult.count;

        this.searchRequest.filters = searchResult.filters;
      }
    );
  }

  public search(): void {
    this.loadActivities();
  }
}
