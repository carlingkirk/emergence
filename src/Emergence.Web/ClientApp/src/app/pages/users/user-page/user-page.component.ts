import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { ContactsService } from 'src/app/service/contacts-service';
import { UserService } from 'src/app/service/user-service';
import { onImgError } from 'src/app/shared/common';
import { Viewer } from 'src/app/shared/interface/viewer';
import { Visibility } from 'src/app/shared/models/enums';
import { Photo } from 'src/app/shared/models/photo';
import { User } from 'src/app/shared/models/user';
import { UserContactRequest } from 'src/app/shared/models/user-contacts';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent extends Viewer {

  @Input()
  public userName: string;
  @Input()
  public id: number;
  public user: User;
  
  constructor(
    authorizeService: AuthorizeService,
    private readonly userService: UserService,
    private readonly contactsService: ContactsService,
    route: ActivatedRoute) {
      super(authorizeService, route);
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (!this.userName) {
      this.userName = this.route.snapshot.params['userName'];
    }
    
    if (!isNaN(+this.userName)) {
      this.id = +this.userName;
    }
    
    if (this.id) {
      this.userService.getUserAsync(this.id).subscribe((user) => {
        this.user = user;
        return of({});
      });
    }
    else if (this.userName) {
      this.userService.getUserByNameAsync(this.userName).subscribe((user) => {
        this.user = user;
        return of({});
      });
    }
  }

  addContactRequestAsync() {
    var request = new UserContactRequest();
    request.userId = this.user.id;
    this.contactsService.addContactRequestAsync(request).subscribe((userContactRequest) => {
      this.user.isViewerContactRequested = true;  
      return of({});
    });
  }

  userIsSelf() {
    return this.userId.toLowerCase() === this.user.userId.toLowerCase();
  }

  contactsViewable() {
    return this.user.profileVisibility === Visibility["My Contacts"] && this.user.isViewerContact;
  }

  profileHidden() {
    return this.user.profileVisibility === Visibility["Private"];
  }

  onImgError(event, photo: Photo) {
    onImgError(event, photo);
  }
}
