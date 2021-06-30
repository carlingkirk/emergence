import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { OriginService } from 'src/app/service/origin-service';
import { Viewer } from 'src/app/shared/interface/viewer';
import { Origin } from 'src/app/shared/models/origin';

@Component({
  selector: 'app-origin-viewer',
  templateUrl: './origin-viewer.component.html',
  styleUrls: ['./origin-viewer.component.css']
})
export class OriginViewerComponent extends Viewer {
  @Input()
  public modal: NgbModalRef;
  @Output()
  public originLoaded = new EventEmitter<Origin>();
  @Input()
  public id: number;
  public origin: Origin;
  public isAuthenticated: Observable<boolean>;
  
  constructor(
    authorizeService: AuthorizeService,
    private readonly originService: OriginService,
    route: ActivatedRoute,
    private router: Router) {
      super(authorizeService, route);
     }

  ngOnInit(): void {
    super.ngOnInit();

    this.originService.getOrigin(this.id).subscribe((origin) => {
      this.origin = origin;
      this.originLoaded.emit(this.origin);
      return of({});
    });
  }

  public removeOrigin() {
    this.originService.deleteOrigin(this.id).subscribe(() => {
      this.router.navigate(['/origins/list']);
    });
  }
}
