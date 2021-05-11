import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Photo } from "../shared/models/photo";
import { PhotoType } from "../shared/models/enums";

@Injectable({
    providedIn: 'root'
})
export class PhotoService {
    
    private baseUrl: string;
    private httpClient: HttpClient;

    constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.httpClient = httpClient;
    }

    public uploadPhotos(type: PhotoType) {
        var typeNum = PhotoType[type];
        var content = {};
        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${typeNum}/upload`, content)
        .pipe((photo) => { return photo });
    }

    public uploadPhoto(type: PhotoType, file: File) {
        var formData: any = new FormData();
        formData.append("photo", file);

        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${type}/upload`, formData)
            .pipe((photo) => { return photo });
    }

    public addExternalPhoto(type: PhotoType, url: string) {
        var typeNum = PhotoType[type];
        var photo = { externalUrl: url };

        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${typeNum}/addexternal`, photo)
        .pipe((photo) => { return photo });
    }

    public removePhoto(id: number) {
        return this.httpClient.delete(this.baseUrl + `api/photo/${id}`);
    }
}