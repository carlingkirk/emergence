import { Component, Input, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { Specimen } from '../../../shared/models/specimen';

@Component({
  selector: 'app-specimens-list',
  templateUrl: './specimens-list.component.html',
  styleUrls: ['./specimens-list.component.css']
})
export class SpecimensListComponent extends Listable implements OnInit, IListable   {
  public specimens: Specimen[];
  @Input()
  public showSearch: boolean = true;
  @Input()
  public forUserId: string;

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
    if (!this.forUserId) {
      this.storageService.getItem("specimen-search").then((searchRequest) => {
        if (!searchRequest) {
          this.resetSearch();
        } else {
          this.searchRequest = searchRequest;
        }
      });
    } else {
      this.searchRequest = {
        filters: null,
        take: 12,
        skip: 0,
        useNGrams: false,
        sortDirection: 0,
        sortBy: null,
        createdBy: this.forUserId
      };
    }
    this.loadSpecimens();
  }

  loadSpecimens() {
    if (!this.forUserId) {
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
    } else {
      this.specimenService.findSpecimens(this.searchRequest).subscribe(
        (searchResult) => {
          this.searchResult = searchResult;
          this.specimens = searchResult.results;
          this.totalCount = searchResult.count;

          this.searchRequest.filters = searchResult.filters;
        }
      );
    }
  }

  public search(): void {
    this.loadSpecimens();
  }
}
