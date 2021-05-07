import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { LifeformService } from 'src/app/service/lifeform-service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { InventoryItemStatus, ItemType, SpecimenStage, Visibility } from 'src/app/shared/models/enums';
import { Lifeform } from 'src/app/shared/models/lifeform';
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
  private inventoryItemStatusEnum = InventoryItemStatus;

  constructor(
    private readonly specimenService: SpecimenService,
    private readonly lifeformService: LifeformService,
    private route: ActivatedRoute
    ) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];

    this.inventoryItemStatuses = Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    this.specimenStages = Object.keys(SpecimenStage).filter(key => !isNaN(Number(key))).map(key => SpecimenStage[key]);
    this.itemTypes = Object.keys(ItemType).filter(key => !isNaN(Number(key))).map((key) => ItemType[key]);

    if (!this.specimen) {
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
        this.specimen = specimen;
      });
    }
  }

  formatter = (result: Lifeform) => result.scientificName;
  inputFormatter = (x: Lifeform) => x.scientificName;

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

  public search: OperatorFunction<string, readonly Lifeform[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? []
        : this.searchLifeforms(term).pipe((lifeform) => lifeform ))
    );

  searchX: OperatorFunction<string, readonly Lifeform[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term =>
        this.searchLifeforms(term).pipe(
          tap(() => this.searchFailed = false),
          catchError(() => {
            this.searchFailed = true;
            return of([]);
          }))
      ),
      tap(() => this.searching = false)
    )

  fruits = ["Apple", "Orange", "Banana"];

  searchY: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? []
        : this.fruits)
    )

  searchZ = (text: Observable<string>) => {
      return text.pipe(
        debounceTime(200),
        distinctUntilChanged(),
        map(term => {
          return term.length < 3 ? [].slice() : this.fruits;
        })
      );
    }

  public saveSpecimen(): void {

  }

  public populateInventoryItemName(): void {

  }

  public cancel(): void {

  }

  public closeModal(): void {
    
  }
}
