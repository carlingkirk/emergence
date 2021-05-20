import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PlantInfo } from "../shared/models/plant-info";
import { SearchRequest } from "../shared/models/search-request";
import { SearchResult } from "../shared/models/search-result";

@Injectable({
    providedIn: 'root'
})
export class PlantInfoService {
    
    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public getPlantInfo(id: number) {
        return this.httpClient.get<PlantInfo>(this.baseUrl + 'api/plantinfo/' + id)
        .pipe((plantInfo) => { return plantInfo });
    }

    public savePlantInfo(plantInfo: PlantInfo) {
        return this.httpClient.put<PlantInfo>(this.baseUrl + 'api/plantinfo', plantInfo)
        .pipe((plantInfo) => { return plantInfo });
    }

    public findPlantInfos(searchRequest: SearchRequest): Observable<SearchResult> {
        return this.httpClient.post<SearchResult>(this.baseUrl + 'api/plantinfo/find', searchRequest)
        .pipe((searchResult) => { return searchResult });
    }
}