import { Location } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { LifeformService } from 'src/app/service/lifeform-service';
import { OriginService } from 'src/app/service/origin-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { onImgError } from 'src/app/shared/common';
import { Editor } from 'src/app/shared/interface/editor';
import { InventoryItemStatus, ItemType, SpecimenStage, Visibility } from 'src/app/shared/models/enums';
import { Lifeform } from 'src/app/shared/models/lifeform';
import { Origin } from 'src/app/shared/models/origin';
import { Photo } from 'src/app/shared/models/photo';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { newSpecimen, Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-edit',
  templateUrl: './specimen-edit.component.html',
  styleUrls: ['./specimen-edit.component.css']
})
export class SpecimenEditComponent extends Editor {
  @Input()
  public specimen: Specimen;
  @Input()
  public modal: NgbModalRef;
  @Input()
  public lifeform: Lifeform;
  @Input()
  public id: number;
  public inventoryItemStatuses: InventoryItemStatus[];
  public specimenStages: SpecimenStage[];
  public itemTypes: ItemType[];
  public searching: boolean;
  public searchFailed: boolean;
  public lifeforms: Lifeform[];
  public selectedLifeform: Lifeform;
  public origins: Origin[];
  public selectedOrigin: Origin;
  public uploadedPhotos: Photo[];
  public itemTypeEnum = ItemType;
  public specimenStageEnum = SpecimenStage;
  public statusEnum = InventoryItemStatus;
  public visibilityEnum = Visibility;
  public editingLifeform: boolean;
  public editingOrigin: boolean;

  constructor(
    authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private readonly lifeformService: LifeformService,
    private readonly originService: OriginService,
    private router: Router,
    route: ActivatedRoute,
    private location: Location,
    private modalService: NgbModal
    ) {
      super(authorizeService, route);
     }

  ngOnInit(): void {
    super.ngOnInit();

    this.inventoryItemStatuses = Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
    this.specimenStages = Object.keys(SpecimenStage).filter(key => !isNaN(Number(key))).map(key => SpecimenStage[key]);
    this.itemTypes = Object.keys(ItemType).filter(key => !isNaN(Number(key))).map((key) => ItemType[key]);

    this.loadSpecimen();
  }

  lifeformResultFormatter = (result: Lifeform) => result.scientificName;
  lifeformInputFormatter = (x: Lifeform) => x.scientificName;
  originResultFormatter = (result: Origin) => result.name;
  originInputFormatter = (x: Origin) => x.name;

  loadSpecimen() {
    if (this.specimen) {
      this.selectedLifeform = this.specimen.lifeform;
      this.selectedOrigin = this.specimen.inventoryItem.origin;
      this.uploadedPhotos = this.specimen.photos;
    }

    if (!this.specimen && this.id > 0) {
      this.specimenService.getSpecimen(this.id).subscribe(
        (specimen) => {
          this.specimen = specimen;
          this.selectedLifeform = specimen.lifeform;
          this.selectedOrigin = specimen.inventoryItem.origin;
          this.uploadedPhotos = specimen.photos;
          return of({});
        },
        (error) => console.log(error),
        () => {});
    }

    if (this.id == 0 && !this.specimen) {
      this.specimen = newSpecimen(this.userId, this.lifeform);
      this.selectedLifeform = this.lifeform;
    }
  }

  searchLifeforms(searchText: string): Observable<Lifeform[]> {
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

    return this.lifeformService.findLifeforms(searchRequest).pipe(map(
      (searchResult) => searchResult.results));
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

  public lifeformsTypeahead: OperatorFunction<string, readonly Lifeform[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchLifeforms(term).pipe((lifeform) => lifeform ))
    )

  public originsTypeahead: OperatorFunction<string, readonly Origin[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchOrigins(term).pipe((origin) => origin ))
    )

  public saveSpecimen(): void {
    this.populateInventoryItemName();
    this.specimen.lifeform = this.selectedLifeform;
    this.specimen.inventoryItem.origin = this.selectedOrigin;
    this.specimen.inventoryItem.quantity = this.specimen.quantity;
    this.specimen.inventoryItem.name = this.specimen.name;
    this.specimen.photos = this.uploadedPhotos;

    this.specimenService.saveSpecimen(this.specimen).subscribe(
      (specimen) => {
        specimen.lifeform = this.selectedLifeform;
        this.modal ? this.modal.close(specimen) : this.router.navigate(['/specimens/', specimen.specimenId]);
      },
      (error) => {
        console.log(error);
        this.errorMessage = 'There was an error saving the specimen';
      });
  }

  public populateInventoryItemName(): void {
    if (!this.specimen.name) {
      this.specimen.name = this.selectedLifeform.scientificName;
    }
  }

  public cancel(): void {
    if (this.specimen.specimenId) {
      this.router.navigate(['/specimens/', this.specimen.specimenId]);
    } else {
      this.location.back();
    }
  }

  showOriginModal(content, name) {
    if (name) {
      this.selectedOrigin = new Origin();
      this.selectedOrigin.name = name;
    }
    
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title', size: 'lg'})
      .result.then((origin) => {
      this.selectedOrigin = origin;
      this.editingOrigin = false;
    });
  }

  public closeModal(): void {
    this.modal.dismiss();
  }

  editOrigin() {
    this.editingOrigin = true;
  }

  cancelEditOrigin(clear: boolean) {
    if (clear) {
      this.selectedOrigin = null;
    }
    this.editingOrigin = false;
  }

  editLifeform() {
    this.editingLifeform = true;
  }

  cancelEditLifeform(clear: boolean) {
    if (clear) {
      this.selectedLifeform = null;
    }
    this.editingLifeform = false;
  }

  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }

   photosChanged(photos: Photo[]) {
    this.uploadedPhotos = photos;
  }
}
