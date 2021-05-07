import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SearchRequest } from "../shared/models/search-request";
import { SearchResult } from "../shared/models/search-result";

@Injectable({
    providedIn: 'root'
})
export class LifeformService {
    
    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public findLifeforms(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/lifeform/find', searchRequest)
        .pipe((searchResult) => { return searchResult });
    }
}