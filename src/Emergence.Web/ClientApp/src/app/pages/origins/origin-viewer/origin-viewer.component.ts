import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { OriginService } from 'src/app/service/origin-service';
import { Origin } from 'src/app/shared/models/origin';

@Component({
  selector: 'app-origin-viewer',
  templateUrl: './origin-viewer.component.html',
  styleUrls: ['./origin-viewer.component.css']
})
export class OriginViewerComponent implements OnInit {
  
  @Input()
  public id: number;
  public origin: Origin;
  public commonName: string;
  public scientificName: string;
  public isEditing: boolean = false;
  public isOwner: boolean = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly originService: OriginService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];

      this.originService.getOrigin(this.id).subscribe((origin) => {
        this.origin = origin;
        this.isOwner = this.origin.createdBy == this.user.userId;
      });
    });
  }
}
