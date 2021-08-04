import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';
import { User, UserSummary } from '../shared/models/user';

@Injectable({
    providedIn: 'root'
})
export class MessageService {

    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public findMessagesAsync(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/message/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }

    public findSentMessagesAsync(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/message/sent/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }
}
