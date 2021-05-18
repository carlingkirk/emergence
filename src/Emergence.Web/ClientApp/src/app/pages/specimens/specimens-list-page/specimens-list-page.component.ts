import { Component, Input, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { Sortable } from 'src/app/shared/interface/sortable';
import { Filter, FilterBody } from 'src/app/shared/models/filters';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { SearchResult } from 'src/app/shared/models/search-result';
import { Specimen } from '../../../shared/models/specimen';

@Component({
  selector: 'app-specimens-list-page',
  templateUrl: './specimens-list-page.component.html',
  styleUrls: ['./specimens-list-page.component.css']
})
export class SpecimensListPageComponent implements OnInit, Sortable {
  public searchRequest: SearchRequest;
  public searchResult: SearchResult;
  public specimens: Specimen[];
  public totalCount: number = 0;
  public forUserId?: string;
  @Input()
  public showSearch: boolean = true;
  public searchText: string;
  public showPublic: boolean;
  public listView: boolean;
  public showFilters: boolean;

  constructor(
    private readonly specimenService: SpecimenService,
    private readonly storageService: StorageService
  ) { }
  public columns: Column[] = [
    { name: 'Scientific Name', value: 'scientificName'},
    { name: 'Common Name', value: 'name'},
    { name: 'Quantity', value: 'quantity'},
    { name: 'Stage', value: 'specimenStage'},
    { name: 'Status', value: 'status'},
    { name: 'Acquired', value: 'dateAcquired'},
    { name: 'Origin', value: 'origin'}
  ];

  ngOnInit(): void {
    this.storageService.getItem("specimen-search").then((searchRequest) => {
      if (!searchRequest) {
        this.resetSearch();
      } else {
        this.searchRequest = searchRequest;
      }
      this.loadSpecimens();
    });
  }

  public resetSearch() {
    this.searchRequest = {
      filters: null,
      take: 12,
      skip: 0,
      useNGrams: false
    };
    this.loadSpecimens();
  }

  loadSpecimens() {
    this.storageService.setItem("specimen-search", this.searchRequest).then((result) => {
      this.specimenService.findSpecimens(this.searchRequest).subscribe(
        (searchResult) => {
          this.searchResult = searchResult;
          this.specimens = searchResult.results;
          this.totalCount = searchResult.count;

          this.searchRequest.filters = searchResult.filters;
        }
      );
    });
  }

  public sort(): void {
    throw new Error('Method not implemented.');
  }

  public toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  public search(): void {
    this.loadSpecimens();
  }

  public filtersChange(filterBody: FilterBody) {
    this.searchRequest.filters = filterBody;
  }
}
