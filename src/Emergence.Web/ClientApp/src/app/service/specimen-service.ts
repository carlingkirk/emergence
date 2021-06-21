import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchRequest } from '../shared/models/search-request';
import { SearchResult } from '../shared/models/search-result';
import { Specimen } from '../shared/models/specimen';

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

    public getSpecimen(id: number) {
        return this.httpClient.get<Specimen>(this.baseUrl + 'api/specimen/' + id)
       // .pipe((specimen) => { return specimen });
        .pipe(specimen => specimen );
    }

    public saveSpecimen(specimen: Specimen) {
        return this.httpClient.put<Specimen>(this.baseUrl + 'api/specimen', specimen)
        .pipe(specimenResponse => specimenResponse);
    }

    public findSpecimens(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/specimen/find', searchRequest)
        .pipe((searchResult) => searchResult);
    }

    public deleteSpecimen(id: number) {
        return this.httpClient.delete(this.baseUrl + 'api/plantinfo/' + id);
    }
}
