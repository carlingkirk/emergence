import { Component, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { Sortable } from 'src/app/shared/interface/sortable';
import { SearchRequest } from 'src/app/shared/models/search-request';
import { SearchResult } from 'src/app/shared/models/search-result';
import { Specimen } from '../../shared/models/specimen';

@Component({
  selector: 'app-specimens-list-page',
  templateUrl: './specimens-list-page.component.html',
  styleUrls: ['./specimens-list-page.component.css']
})
export class SpecimensListPageComponent implements OnInit, Sortable {
  public searchResult: SearchResult;
  public specimens: Specimen[];
  public totalCount: number = 0;
  public forUserId?: string;

  constructor(
    private readonly specimenService: SpecimenService
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
    this.loadSpecimens();
  }

  loadSpecimens() {
    var searchRequest: SearchRequest = {
      take: 12,
      skip: 0,
      useNGrams: false
    }
    this.specimenService.findSpecimens(searchRequest).subscribe(
      (searchResult) => {
        this.searchResult = searchResult;
        this.specimens = searchResult.results;
        this.totalCount = searchResult.count;
      }
    );
  }

  Sort(): void {
    throw new Error('Method not implemented.');
  }
}
