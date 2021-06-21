import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { PhotoType } from '../shared/models/enums';
import { Photo } from '../shared/models/photo';

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
        const typeNum = PhotoType[type];
        const content = {};
        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${typeNum}/upload`, content)
        .pipe((photo) => photo);
    }

    public uploadPhoto(type: PhotoType, file: File) {
        const formData: any = new FormData();
        formData.append('photo', file);

        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${type}/upload`, formData)
            .pipe((photo) => photo);
    }

    public addExternalPhoto(type: PhotoType, url: string) {
        const typeNum = PhotoType[type];
        const externalPhoto = { externalUrl: url };

        return this.httpClient.post<Photo>(this.baseUrl + `api/photo/${typeNum}/addexternal`, externalPhoto)
        .pipe(photo => photo);
    }

    public removePhoto(id: number) {
        return this.httpClient.delete(this.baseUrl + `api/photo/${id}`);
    }
}
