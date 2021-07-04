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
  @Input()
  public id: number;
  @Input()
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

    if (this.id > 0 && !this.origin) {
      this.originService.getOrigin(this.id).subscribe((origin) => {
        this.origin = origin;
        return of({});
      });
    } else {
      this.id = this.origin.originId;
    }
  }

  public removeOrigin() {
    this.originService.deleteOrigin(this.id).subscribe(() => {
      this.router.navigate(['/origins/list']);
    });
  }
}
