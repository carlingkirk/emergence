import { basename } from '@angular/compiler-cli/src/ngtsc/file_system';
import { Component, Input, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { FilterBody } from 'src/app/shared/models/filters';
import { PageRequest, SearchRequest, SortRequest } from 'src/app/shared/models/search-request';
import { SearchResult } from 'src/app/shared/models/search-result';
import { Specimen } from '../../../shared/models/specimen';

@Component({
  selector: 'app-specimens-list-page',
  templateUrl: './specimens-list-page.component.html',
  styleUrls: ['./specimens-list-page.component.css']
})
export class SpecimensListPageComponent extends Listable implements OnInit, IListable   {
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
  ) { 
    super(); 
  }
  public columns: Column[] = [
    { name: 'Scientific Name', value: 'ScientificName'},
    { name: 'Common Name', value: 'CommonName'},
    { name: 'Quantity', value: 'Quantity'},
    { name: 'Stage', value: 'Stage'},
    { name: 'Status', value: 'Status'},
    { name: 'Acquired', value: 'DateAcquired'},
    { name: 'Origin', value: 'Origin'}
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

  public search(): void {
    this.loadSpecimens();
  }
}
