import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { LifeformService } from 'src/app/service/lifeform-service';
import { OriginService } from 'src/app/service/origin-service';
import { PlantInfoService } from 'src/app/service/plant-info-service';
import { getElementId, onImgError } from 'src/app/shared/common';
import { Effect, LightType, Month, SoilType, StratificationType, Visibility, WaterType, Wildlife } from 'src/app/shared/models/enums';
import { Lifeform } from 'src/app/shared/models/lifeform';
import { Origin } from 'src/app/shared/models/origin';
import { Photo } from 'src/app/shared/models/photo';
import { PlantInfo, StratificationStage, WildlifeEffect, Zone } from 'src/app/shared/models/plant-info';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { getZones } from 'src/app/shared/models/zone';

@Component({
  selector: 'app-plant-info-edit',
  templateUrl: './plant-info-edit.component.html',
  styleUrls: ['./plant-info-edit.component.css']
})
export class PlantInfoEditComponent implements OnInit {

  @Input()
  public plantInfo: PlantInfo;
  @Input()
  public id: number;
  public visibilities: Visibility[];
  public searching: boolean;
  public searchFailed: boolean;
  public lifeforms: Lifeform[];
  public selectedLifeform: Lifeform;
  public origins: Origin[];
  public selectedOrigin: Origin;
  public user: IUser;
  public uploadedPhotos: Photo[];
  public chosenStratificationStages: StratificationStage[];
  public chosenWildlifeEffects: WildlifeEffect[];
  public chosenSoilTypes: SoilType[];
  public lightTypes: LightType[];
  public waterTypes: WaterType[];
  public zones: Zone[] = getZones();
  public months: Month[];
  public stratificationTypes: StratificationType[];
  public wildlifeTypes: Wildlife[];
  public effects: Effect[];
  public soilTypes: SoilType[];
  public minimumZoneId: number;
  public maximumZoneId: number;
  public errorMessage: string;
  
  constructor(
    private authorizeService: AuthorizeService,
    private readonly plantInfoService: PlantInfoService,
    private readonly lifeformService: LifeformService,
    private readonly originService: OriginService,
    private router: Router,
    private route: ActivatedRoute
    ) { }

  lifeformResultFormatter = (result: Lifeform) => result.scientificName;
  lifeformInputFormatter = (x: Lifeform) => x.scientificName;
  originResultFormatter = (result: Origin) => result.name;
  originInputFormatter = (x: Origin) => x.name;

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    this.lightTypes = Object.keys(LightType).filter(key => !isNaN(Number(key))).map(key => LightType[key]);
    this.waterTypes = Object.keys(WaterType).filter(key => !isNaN(Number(key))).map(key => WaterType[key]);
    this.months = Object.keys(Month).filter(key => !isNaN(Number(key))).map(key => Month[key]);
    this.stratificationTypes = Object.keys(StratificationType).filter(key => !isNaN(Number(key))).map(key => StratificationType[key]);
    this.wildlifeTypes = Object.keys(Wildlife).filter(key => !isNaN(Number(key))).map(key => Wildlife[key]);
    this.effects = Object.keys(Effect).filter(key => !isNaN(Number(key))).map(key => Effect[key]);
    this.soilTypes = Object.keys(SoilType).filter(key => !isNaN(Number(key))).map(key => SoilType[key]);
    this.zones = getZones();
    
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];
      this.loadPlantInfo();
    });
  }

  loadPlantInfo() {
    if (!this.plantInfo && this.id > 0) {
      this.plantInfoService.getPlantInfo(this.id).subscribe((plantInfo) => {
        this.plantInfo = plantInfo;
        this.selectedLifeform = plantInfo.lifeform;
        this.selectedOrigin = plantInfo.origin;
        this.uploadedPhotos = plantInfo.photos;
        this.chosenStratificationStages = plantInfo.requirements.stratificationStages;
        this.chosenWildlifeEffects = plantInfo.wildlifeEffects;
        this.chosenSoilTypes = plantInfo.soilTypes ?? [];
      });
    }

    if (this.id == 0) {
      this.plantInfo = new PlantInfo();
      this.plantInfo.createdBy = this.user.userId;
      this.plantInfo.dateCreated = new Date();
      this.plantInfo.photos = [];
      this.plantInfo.visibility = this.visibilities[Visibility["Inherit from profile"]];
      this.chosenSoilTypes = [];
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
        : this.searchOrigins(term).pipe((origin) => origin ))
    );

  public savePlantInfo(): void {
    this.plantInfo.lifeform = this.selectedLifeform;
    this.plantInfo.commonName = this.selectedLifeform.commonName;
    this.plantInfo.scientificName = this.selectedLifeform.scientificName;
    this.plantInfo.origin = this.selectedOrigin;
    this.plantInfo.photos = this.uploadedPhotos;
    this.plantInfo.requirements.stratificationStages = this.chosenStratificationStages;
    this.plantInfo.wildlifeEffects = this.chosenWildlifeEffects;
    this.plantInfo.soilTypes = this.chosenSoilTypes;

    this.plantInfoService.savePlantInfo(this.plantInfo).subscribe(
      (plantInfo) => this.router.navigate(['/plantinfos/', plantInfo.plantInfoId]),
      (error) => {
        console.log(error);
        this.errorMessage = "There was an error saving the plant profile";
      });
  }

  public cancel(): void {
    if (this.plantInfo.plantInfoId) {
      this.router.navigate(['/plantinfos/', this.plantInfo.plantInfoId]);
    } else {
      this.router.navigate([".."]);
    }
  }
  
  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }

   photosChanged(photos: Photo[]) {
    this.uploadedPhotos = photos;
  }

  getElementId(element: string, id: string) {
    return getElementId(element, id);
  }

  addStratificationStage() {
    if (!this.chosenStratificationStages) {
      this.chosenStratificationStages = [];
    }
    this.chosenStratificationStages.push({ 
        step: this.chosenStratificationStages.length + 1, 
        dayLength: null, 
        stratificationType: null
      });
  }

  removeStratificationStage(step: number) {
    this.chosenStratificationStages = this.chosenStratificationStages.filter((stage) => stage.step != step);
    if (this.chosenStratificationStages.length === 0) {
      this.chosenStratificationStages = null;
    }
  }

  addWildlifeEffect() {
    if (!this.chosenWildlifeEffects) {
      this.chosenWildlifeEffects = [];
    }
    this.chosenWildlifeEffects.push({ 
        effect: null,
        wildlife: null
      });
  }

  removeWildlifeEffect(index: number) {
    this.chosenWildlifeEffects.splice(index, 1);
    if (this.chosenWildlifeEffects.length === 0) {
      this.chosenWildlifeEffects = null;
    }
  }

  isSoilTypeChosen(soilType: SoilType) {
    return this.chosenSoilTypes.includes(soilType);
  }

  addRemoveSoilType(soilType: SoilType) {
    if (this.isSoilTypeChosen(soilType)) {
      this.chosenSoilTypes = this.chosenSoilTypes.filter((type) => type != soilType);
    } else {
      this.chosenSoilTypes.push(soilType);
    }

    if (this.chosenSoilTypes.length === 0) {
      this.chosenSoilTypes = null;
    }
  }
}
