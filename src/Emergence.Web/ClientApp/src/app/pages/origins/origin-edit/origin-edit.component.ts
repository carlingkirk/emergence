import { Location } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { OriginService } from 'src/app/service/origin-service';
import { Editor } from 'src/app/shared/interface/editor';
import { OriginType, Visibility } from 'src/app/shared/models/enums';
import { GeoLocation } from 'src/app/shared/models/location';
import { Origin } from 'src/app/shared/models/origin';
import { SearchRequest } from 'src/app/shared/models/search-request';

@Component({
  selector: 'app-origin-edit',
  templateUrl: './origin-edit.component.html',
  styleUrls: ['./origin-edit.component.css']
})
export class OriginEditComponent extends Editor {

  @Input()
  public origin: Origin;
  @Input()
  public modal: NgbModalRef;
  @Input()
  public id: number;
  public origins: Origin[];
  public selectedParentOrigin: Origin;
  public originTypes: OriginType[];
  public editingOrigin: boolean;

  constructor(
    authorizeService: AuthorizeService,
    private readonly originService: OriginService,
    private router: Router,
    route: ActivatedRoute,
    private location: Location,
    private modalService: NgbModal
  ) {
    super(authorizeService, route);
   }

  originResultFormatter = (result: Origin) => result.name;
  originInputFormatter = (x: Origin) => x.name;

  ngOnInit(): void {
    super.ngOnInit();
    
    this.originTypes = Object.keys(OriginType).filter(key => !isNaN(Number(key))).map(key => OriginType[key]);

    this.loadOrigin();
  }

  loadOrigin() {
    if (this.origin) {
      this.selectedParentOrigin = this.origin.parentOrigin;

      if (!this.origin.location) {
        this.origin.location = new GeoLocation();
      }
    }

    if (!this.origin && this.id > 0) {
      this.originService.getOrigin(this.id).subscribe((origin) => {
        this.origin = origin;
        this.selectedParentOrigin = origin.parentOrigin;

        if (!this.origin.location) {
          this.origin.location = new GeoLocation();
        }
        return of({});
      });
    }

    if (this.id == 0) {
      this.origin = new Origin();
      this.origin.createdBy = this.userId;
      this.origin.dateCreated = new Date();
      this.origin.visibility = this.visibilities[Visibility['Inherit from profile']];
      this.origin.location = new GeoLocation();
    }
  }

  searchOrigins(searchText: string): Observable<Origin[]> {
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

    return this.originService.findOrigins(searchRequest).pipe(map(
      (searchResult) => {
        let newOrigin = new Origin();
        newOrigin.originId = 0;
        newOrigin.name = searchText;
        let results = searchResult.results as Origin[];
        results.push(newOrigin);
        return searchResult.results;
      }));
  }

  public originsTypeahead: OperatorFunction<string, readonly Origin[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchOrigins(term).pipe((origin) => origin ))
  );

  showOriginModal(content, name) {
    if (name) {
      this.selectedParentOrigin = new Origin();
      this.selectedParentOrigin.name = name;
    }
    
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title', size: 'lg'})
      .result.then((origin) => {
      this.selectedParentOrigin = origin;
      this.editingOrigin = false;
    });
  }

  editOrigin() {
    this.editingOrigin = true;
  }

  cancelEditOrigin(clear: boolean) {
    if (clear) {
      this.selectedParentOrigin = null;
    }
    this.editingOrigin = false;
  }

  public saveOrigin(): void {
    if (!this.selectedParentOrigin) {
      this.origin.parentOrigin = null;
    } else {
      this.origin.parentOrigin = this.selectedParentOrigin;
    }

    this.originService.saveOrigin(this.origin).subscribe(
      (origin) => this.modal ? this.modal.close(origin) : this.router.navigate(['/origins/', origin.originId]),
      (error) => {
        console.log(error);
        this.errorMessage = 'There was an error saving the origin';
      });
  }

  public cancel(): void {
    if (this.origin.originId) {
      this.router.navigate(['/origins/', this.origin.originId]);
    } else {
      this.location.back();
    }
  }

  public closeModal(): void {
    this.modal.dismiss();
  }
}
