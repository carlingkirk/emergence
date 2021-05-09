import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { LifeformService } from 'src/app/service/lifeform-service';
import { OriginService } from 'src/app/service/origin-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { InventoryItemStatus, ItemType, SpecimenStage, Visibility } from 'src/app/shared/models/enums';
import { Lifeform } from 'src/app/shared/models/lifeform';
import { Origin } from 'src/app/shared/models/origin';
import { Photo } from 'src/app/shared/models/photo';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-edit',
  templateUrl: './specimen-edit.component.html',
  styleUrls: ['./specimen-edit.component.css']
})
export class SpecimenEditComponent implements OnInit {
  @Input()
  public specimen: Specimen;
  @Input()
  public id: number;
  public inventoryItemStatuses: InventoryItemStatus[];
  public visibilities: Visibility[];
  public specimenStages: SpecimenStage[];
  public itemTypes: ItemType[];
  public modal: boolean;
  public searching: boolean;
  public searchFailed: boolean;
  public lifeforms: Lifeform[];
  public selectedLifeform: Lifeform;
  public origins: Origin[];
  public selectedOrigin: Origin;
  public user: IUser;
  public uploadedPhotos: Photo[];

  constructor(
    private authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private readonly lifeformService: LifeformService,
    private readonly originService: OriginService,
    private route: ActivatedRoute
    ) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];

    this.inventoryItemStatuses = Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    this.specimenStages = Object.keys(SpecimenStage).filter(key => !isNaN(Number(key))).map(key => SpecimenStage[key]);
    this.itemTypes = Object.keys(ItemType).filter(key => !isNaN(Number(key))).map((key) => ItemType[key]);
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];
      this.loadSpecimen();
    });
  }

  lifeformResultFormatter = (result: Lifeform) => result.scientificName;
  lifeformInputFormatter = (x: Lifeform) => x.scientificName;
  originResultFormatter = (result: Origin) => result.name;
  originInputFormatter = (x: Origin) => x.name;

  loadSpecimen() {
    if (!this.specimen && this.id > 0) {
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
        this.specimen = specimen;
        this.selectedLifeform = specimen.lifeform;
        this.selectedOrigin = specimen.inventoryItem.origin;
        this.uploadedPhotos = specimen.photos;
      });
    }

    if (this.id == 0) {
      this.specimen.createdBy = this.user.userId;
      this.specimen.ownerId = this.user.userId;
      this.specimen.dateCreated = new Date();
      this.specimen.inventoryItem.inventory.createdBy = this.user.userId;
      this.specimen.inventoryItem.inventory.dateCreated = new Date();
      this.specimen.inventoryItem.createdBy = this.user.userId;
      this.specimen.inventoryItem.dateCreated = new Date();
      this.specimen.inventoryItem.itemType = ItemType.Specimen;
    }
  }

  searchLifeforms(searchText: string): Observable<Lifeform[]> {
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

    return this.lifeformService.findLifeforms(searchRequest).pipe(map(
      (searchResult) => searchResult.results));
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

  public lifeformsTypeahead: OperatorFunction<string, readonly Lifeform[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchLifeforms(term).pipe((lifeform) => lifeform ))
    );

  public originsTypeahead: OperatorFunction<string, readonly Origin[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchLifeforms(term).pipe((lifeform) => lifeform ))
    );

  public saveSpecimen(): void {
    this.specimen.lifeform = this.selectedLifeform;
    this.specimen.photos = this.uploadedPhotos;
  }

  public populateInventoryItemName(): void {

  }

  public cancel(): void {

  }

  public closeModal(): void {
    
  }

  onImgError(event, photo: Photo) {
    event.onerror = null;
    event.srcset = '';
    event.src = photo.originalUri;
    event.target.src = photo.originalUri;
   }
}
