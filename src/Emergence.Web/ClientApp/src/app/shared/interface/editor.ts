import { Injectable, Input, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Observable, Subscription } from "rxjs";
import { tap } from "rxjs/operators";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { getElementId } from "../common";
import { Visibility } from "../models/enums";

export interface IEditor {
    userId: string;
    getUserSub: Subscription;
}

@Injectable()
export abstract class Editor implements IEditor, OnInit, OnDestroy {
    @Input()
    public id: number;
    public userId: string;
    public getUserSub: Subscription;
    public isAuthenticated: Observable<boolean>;
    public visibilities: Visibility[];
    public errorMessage: string;

    constructor(
        private authorizeService: AuthorizeService,
        protected route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        if (!this.id) {
            this.id = this.route.snapshot.params['id'];
        }
        this.isAuthenticated = this.authorizeService.isAuthenticated();
        this.getUserSub = this.authorizeService.getUserId().pipe(tap(u => this.userId = u)).subscribe();
        this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    }

    getElementId(element: string, id: string) {
        return getElementId(element, id);
      }

    ngOnDestroy(): void {
        this.getUserSub.unsubscribe();
    }
}