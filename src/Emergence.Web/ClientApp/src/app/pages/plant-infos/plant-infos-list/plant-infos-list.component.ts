import { Component, Input, OnInit } from '@angular/core';
import { PlantInfoService } from 'src/app/service/plant-info-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { PlantInfo } from 'src/app/shared/models/plant-info';

@Component({
  selector: 'app-plant-infos-list',
  templateUrl: './plant-infos-list.component.html',
  styleUrls: ['./plant-infos-list.component.css']
})
export class PlantInfosListComponent extends Listable implements OnInit, IListable {
  public plantInfos: PlantInfo[];
  @Input()
  public showSearch: boolean = true;
  
  constructor(
    private readonly plantInfoService: PlantInfoService,
    private readonly storageService: StorageService
  ) { 
    super();
  }

  public columns: Column[] = [
    { name: 'Scientific Name', value: 'ScientificName'},
    { name: 'Common Name', value: 'CommonName'},
    { name: 'Origin', value: 'Origin'},
    { name: 'Zone', value: 'Zone'},
    { name: 'Light', value: 'Light'},
    { name: 'Water', value: 'Water'},
    { name: 'Bloom Time', value: 'BloomTime'},
    { name: 'Height', value: 'Height'},
    { name: 'Spread', value: 'Spread'}
  ];

  ngOnInit(): void {
    this.storageService.getItem("plant-info-search").then((searchRequest) => {
      if (!searchRequest) {
        this.resetSearch();
      } else {
        this.searchRequest = searchRequest;
      }
      this.loadPlantInfos();
    });
  }

  loadPlantInfos() {
    this.storageService.setItem("plant-info-search", this.searchRequest).then((result) => {
      this.plantInfoService.findPlantInfos(this.searchRequest).subscribe(
        (searchResult) => {
          this.searchResult = searchResult;
          this.plantInfos = searchResult.results;
          this.totalCount = searchResult.count;

          this.searchRequest.filters = searchResult.filters;
        }
      );
    });
  }

  public search(): void {
    this.loadPlantInfos();
  }
}
