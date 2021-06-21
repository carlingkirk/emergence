import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { ActivityService } from 'src/app/service/activity-service';
import { Activity } from 'src/app/shared/models/activity';

@Component({
  selector: 'app-activity-viewer',
  templateUrl: './activity-viewer.component.html',
  styleUrls: ['./activity-viewer.component.css']
})
export class ActivityViewerComponent implements OnInit {

  @Input()
  public id: number;
  public activity: Activity;
  public isEditing = false;
  public isOwner = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly activityService: ActivityService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user['sub'];

      this.activityService.getActivity(this.id).subscribe((activity) => {
        this.activity = activity;
        this.isOwner = this.activity.createdBy === this.user.userId;
        return of({});
      });
      return of({});
    });
  }

  public removeActivity() {
    this.activityService.deleteActivity(this.id).subscribe(() => {
      this.router.navigate(['/activities/list']);
    });
  }
}
