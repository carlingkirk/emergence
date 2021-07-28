import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';
import { User, UserSummary } from '../shared/models/user';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public getUserAsync(id: number) {
        return this.httpClient.get<User>(this.baseUrl + 'api/user/get?id=' + id)
            .pipe((user) => user);
    }

    public getUserByNameAsync(userName: string) {
        return this.httpClient.get<User>(this.baseUrl + 'api/user/get?name=' + userName)
            .pipe((user) => user);
    }

    public getUserSummary(id: string) {
        if (id) {
            return this.httpClient.get<UserSummary>(this.baseUrl + 'api/user/summary/' + id)
            .pipe((user) => user);
        }
    }

    public findUsers(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/user/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }
}
