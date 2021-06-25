import { Injectable, Input, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Observable, Subscription } from "rxjs";
import { tap } from "rxjs/operators";
import { AuthorizeService } from "src/api-authorization/authorize.service";

export interface IViewer {
    userId: string;
    getUserSub: Subscription;
}

@Injectable()
export abstract class Viewer implements IViewer, OnInit, OnDestroy {
    @Input()
    public id: number;
    public userId: string;
    public getUserSub: Subscription;
    public isEditing = false;
    public isAuthenticated: Observable<boolean>;

    constructor(
        private authorizeService: AuthorizeService,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        if (!this.id) {
            this.id = this.route.snapshot.params['id'];
        }
        this.isAuthenticated = this.authorizeService.isAuthenticated();
        this.getUserSub = this.authorizeService.getUserId().pipe(tap(u => this.userId = u)).subscribe();
    }

    public edit() {
        this.isEditing = true;
    }

    ngOnDestroy(): void {
        this.getUserSub.unsubscribe();
    }
}