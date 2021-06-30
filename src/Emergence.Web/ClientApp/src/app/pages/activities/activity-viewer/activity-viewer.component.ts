import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ActivityService } from 'src/app/service/activity-service';
import { Viewer } from 'src/app/shared/interface/viewer';
import { Activity } from 'src/app/shared/models/activity';

@Component({
  selector: 'app-activity-viewer',
  templateUrl: './activity-viewer.component.html',
  styleUrls: ['./activity-viewer.component.css']
})
export class ActivityViewerComponent extends Viewer {

  @Input()
  public id: number;
  public activity: Activity;
  public isAuthenticated: Observable<boolean>;

  constructor(
    authorizeService: AuthorizeService,
    private readonly activityService: ActivityService,
    route: ActivatedRoute,
    private router: Router) {
      super(authorizeService, route);
  }

  ngOnInit(): void {
    super.ngOnInit();

    this.activityService.getActivity(this.id).subscribe((activity) => {
      this.activity = activity;
      return of({});
    });
  }

  public removeActivity() {
    this.activityService.deleteActivity(this.id).subscribe(() => {
      this.router.navigate(['/activities/list']);
    });
  }
}
