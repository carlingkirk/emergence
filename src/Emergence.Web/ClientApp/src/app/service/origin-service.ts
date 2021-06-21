import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Origin } from '../shared/models/origin';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';

@Injectable({
    providedIn: 'root'
})
export class OriginService {

    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public getOrigin(id: number) {
        return this.httpClient.get<Origin>(this.baseUrl + 'api/origin/' + id)
        .pipe((origin) => origin);
    }

    public findOrigins(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/origin/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }

    public saveOrigin(origin: Origin) {
        return this.httpClient.put<Origin>(this.baseUrl + 'api/origin/', origin)
        .pipe((originResponse) => originResponse);
    }

    public deleteOrigin(id: number) {
        return this.httpClient.delete(this.baseUrl + 'api/origin/' + id);
    }
}
