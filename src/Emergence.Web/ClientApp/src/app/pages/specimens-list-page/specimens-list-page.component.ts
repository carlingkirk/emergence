import { Component, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { SearchRequest } from 'src/app/shared/search-request';
import { SearchResult } from 'src/app/shared/search-result';
import { Specimen } from '../../shared/specimen';

@Component({
  selector: 'app-specimens-list-page',
  templateUrl: './specimens-list-page.component.html',
  styleUrls: ['./specimens-list-page.component.css']
})
export class SpecimensListPageComponent implements OnInit {
  public searchResult: SearchResult;
  public specimens: Specimen[];
  public totalCount: number = 0;
  public forUserId?: string;

  constructor(
    private readonly specimenService: SpecimenService
  ) { }

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

}
