import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { combineLatest, Observable, of } from 'rxjs';
import { map, mergeMap, tap } from 'rxjs/operators';
import { UserService } from 'src/app/service/user-service';
import { UserSummary } from 'src/app/shared/models/user';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public userId: string;

  public userSummary: UserSummary;

  constructor(private authorizeService: AuthorizeService,
    private userService: UserService) { }

  ngOnInit() {
    this.authorizeService.getUser().pipe(
      tap(u => {
        this.isAuthenticated = of(!!u)
    }),
    map(u => u && u),
    mergeMap((user) => {
      if (user && user.sub !== this.userId) {
        return combineLatest([of(user), this.userService.getUserSummary(user.sub)])
      }
      else return of();
    })).subscribe(([user, userSummary]) => {
      this.userId = user.sub;
      this.userSummary = userSummary;
      return of({});
    });
  }
}
