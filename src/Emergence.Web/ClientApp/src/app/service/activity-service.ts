import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Activity } from "../shared/models/activity";
import { SearchRequest } from "../shared/models/search-request";
import { SearchResult } from "../shared/models/search-result";

@Injectable({
    providedIn: 'root'
})
export class ActivityService {
    
    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public getActivity(id: number) {
        return this.httpClient.get<Activity>(this.baseUrl + 'api/activity/' + id)
        .pipe((activity) => { return activity });
    }

    public findActivities(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/activity/find', searchRequest)
        .pipe((searchResult) => { return searchResult });
    }

    public findSpecimenActivities(searchRequest: SearchRequest, specimenId: number): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + `/api/activity/find?specimenId=${specimenId}`, searchRequest)
        .pipe((searchResult) => { return searchResult });
    }

    public findScheduledActivities(searchRequest: SearchRequest, date: Date): Observable<SearchResult> {
        var utcDate =  Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
        var ticks = new Date(utcDate).getTime() * 10000;
        return this.httpClient.post<SearchResult>(this.baseUrl + `/api/activity/find/scheduled?date=${ticks}`, searchRequest)
        .pipe((searchResult) => { return searchResult });
    }

    public saveActivity(activity: Activity) {
        return this.httpClient.put<Activity>(this.baseUrl + 'api/activity/', activity)
        .pipe((activity) => { return activity });
    }
}