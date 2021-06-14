import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { OriginService } from 'src/app/service/origin-service';
import { OriginType, Visibility } from 'src/app/shared/models/enums';
import { GeoLocation } from 'src/app/shared/models/location';
import { Origin } from 'src/app/shared/models/origin';
import { SearchRequest } from 'src/app/shared/models/search-request';

@Component({
  selector: 'app-origin-edit',
  templateUrl: './origin-edit.component.html',
  styleUrls: ['./origin-edit.component.css']
})
export class OriginEditComponent implements OnInit {

  @Input()
  public origin: Origin;
  @Input()
  public id: number;
  public origins: Origin[];
  public selectedParentOrigin: Origin;
  public originTypes: OriginType[];
  public visibilities: Visibility[];
  public user: IUser;
  public errorMessage: string;
  
  constructor(
    private authorizeService: AuthorizeService,
    private readonly originService: OriginService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  originResultFormatter = (result: Origin) => result.name;
  originInputFormatter = (x: Origin) => x.name;

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.originTypes = Object.keys(OriginType).filter(key => !isNaN(Number(key))).map(key => OriginType[key]);
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);

    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];
      this.loadOrigin();
    });
  }

  loadOrigin() {
    if (!this.origin && this.id > 0) {
      this.originService.getOrigin(this.id).subscribe((origin) => {
        this.origin = origin;
        this.selectedParentOrigin = origin.parentOrigin;

        if (!this.origin.location) {
          this.origin.location = new GeoLocation();
        }
      });
    }

    if (this.id == 0) {
      this.origin = new Origin();
      this.origin.createdBy = this.user.userId;
      this.origin.dateCreated = new Date();
      this.origin.visibility = this.visibilities[Visibility["Inherit from profile"]];
      this.origin.location = new GeoLocation();
    }
  }

  searchOrigins(searchText: string): Observable<Origin[]> {
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

    return this.originService.findOrigins(searchRequest).pipe(map(
      (searchResult) => searchResult.results));
  }

  public originsTypeahead: OperatorFunction<string, readonly Origin[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchOrigins(term).pipe((origin) => origin ))
    );

    public saveOrigin(): void {
      if (!this.selectedParentOrigin) {
        this.origin.parentOrigin = null;
      } else {
        this.origin.parentOrigin = this.selectedParentOrigin;
      }
  
      this.originService.saveOrigin(this.origin).subscribe(
        (origin) => this.router.navigate(['/origins/', origin.originId]),
        (error) => {
          console.log(error);
          this.errorMessage = "There was an error saving the origin";
        });
    }
  
    public cancel(): void {
      if (this.origin.originId) {
        this.router.navigate(['/origins/', this.origin.originId]);
      } else {
        this.router.navigate([".."]);
      }
    }
  
}
