import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
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
  @Input()
  public modal: NgbModalRef;
  @Output()
  public originLoaded = new EventEmitter<Origin>();
  public origin: Origin;
  public commonName: string;
  public scientificName: string;
  public isEditing = false;
  public isOwner = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly originService: OriginService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    if (!this.id) {
      this.id = this.route.snapshot.params['id'];
    }
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user['sub'];

      this.originService.getOrigin(this.id).subscribe((origin) => {
        this.origin = origin;
        this.isOwner = this.origin.createdBy === this.user.userId;
        this.originLoaded.emit(this.origin);
        return of({});
      });
      return of({});
    });
  }

  public removeOrigin() {
    this.originService.deleteOrigin(this.id).subscribe(() => {
      this.router.navigate(['/origins/list']);
    });
  }

  public editOrigin() {
    this.isEditing = true;
  }
}
