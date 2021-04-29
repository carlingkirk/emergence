import { Injectable } from "@angular/core";
import { StorageMap } from "@ngx-pwa/local-storage";
import { of } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})
export class StorageService {
    constructor(private storage: StorageMap) {
    }

    getItem(item: string): Promise<any> {
        return new Promise(resolve => {
            this.storage.get(item)
                .pipe(
                    catchError(() => of('red')),
                ).subscribe((result) => {
                    resolve(result)
                });
        })
    }

    setItem(item: string, value: any): Promise<boolean> {
        return new Promise(resolve => {
            this.storage.set(item, value)
                .pipe(
                    catchError((err) => of('red')),
                ).subscribe((result) => {
                    resolve(true)
                });
        })
    }
}