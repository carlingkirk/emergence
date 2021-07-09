import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { combineLatest, Observable, of, pipe, Subscription } from 'rxjs';
import { map, mergeMap, tap } from 'rxjs/operators';
import { UserService } from 'src/app/service/user-service';
import { UserSummary } from 'src/app/shared/models/user';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public userId: string;
  public userId$: Observable<string>;
  public getUserSub: Subscription;
  public userSummary: UserSummary;

  constructor(private authorizeService: AuthorizeService,
    private userService: UserService) { }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
  }

  ngAfterViewInit(): void {
    this.authorizeService.getUserId().pipe(
      mergeMap(userId => {
        if (userId && userId !== this.userId) {
          return combineLatest([of(userId), this.userService.getUserSummary(userId)])
        }
        else return of();
      })
    ).subscribe(([userId, userSummary]) => {
      this.userId = userId;
      this.userSummary = userSummary;
      return of({});
    });
  }

  ngOnDestroy(): void {
    this.getUserSub.unsubscribe();
  }
}
