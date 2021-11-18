import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';
import { UserContact, UserContactRequest } from '../shared/models/user-contacts';

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

    public addContactRequestAsync(userContactRequest: UserContactRequest): Observable<UserContactRequest> {
        return this.httpClient.post<UserContactRequest>(this.baseUrl + 'api/usercontact/request', userContactRequest)
        .pipe((userContactRequest) => userContactRequest);
    }

    public addContact(userContactRequest: UserContactRequest): Observable<UserContact> {
        return this.httpClient.post<UserContact>(this.baseUrl + 'api/usercontact', userContactRequest)
        .pipe((userContact) => userContact);
    }
}
