import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';

@Injectable({
    providedIn: 'root'
})
export class ContactsService {

    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public findContactsAsync(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/usercontact/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }

    public removeContactAsync(id: number) {
        return this.httpClient.delete(this.baseUrl + 'api/usercontact/' + id);
    }

    public findContactRequestsAsync(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/usercontact/request/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }

    public removeContactRequestAsync(id: number) {
        return this.httpClient.delete(this.baseUrl + 'api/usercontact/request/' + id);
    }
}
