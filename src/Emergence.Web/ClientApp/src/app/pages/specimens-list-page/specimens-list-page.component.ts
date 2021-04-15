import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { SearchRequest } from 'src/app/shared/search-request';
import { SearchResult } from 'src/app/shared/search-result';
import { Specimen } from '../../shared/specimen';

@Component({
  selector: 'app-specimens-list-page',
  templateUrl: './specimens-list-page.component.html',
  styleUrls: ['./specimens-list-page.component.css']
})
export class SpecimensListPageComponent implements OnInit {
  private baseUrl: string;
  private httpClient: HttpClient;
  public specimens: Specimen[];
  public totalCount: number = 0;
  public forUserId?: string;

  constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.httpClient = httpClient;
  }

  ngOnInit(): void {
    this.loadSpecimens();
  }

  loadSpecimens() {
    var searchRequest: SearchRequest = {
      take: 12,
      skip: 0,
      useNGrams: false
    }
    this.httpClient.post<SearchResult>(this.baseUrl + 'api/specimen/find', searchRequest).subscribe(result => {
      this.specimens = result.results;
      this.totalCount = result.count;
    }, error => console.error(error));
  }

}
