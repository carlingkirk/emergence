import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SearchRequest } from "../shared/search-request";
import { SearchResult } from "../shared/search-result";

@Injectable({
    providedIn: 'root'
})
export class SpecimenService {
    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public findSpecimens(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/specimen/find', searchRequest)
        .pipe((searchResult) => { return searchResult });
    }
}