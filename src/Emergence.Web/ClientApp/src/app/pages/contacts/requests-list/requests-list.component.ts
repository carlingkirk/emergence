import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ContactsService } from 'src/app/service/contacts-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { UserContact, UserContactRequest } from 'src/app/shared/models/user-contacts';

@Component({
  selector: 'app-requests-list',
  templateUrl: './requests-list.component.html',
  styleUrls: ['./requests-list.component.css']
})
export class RequestsListComponent extends Listable implements OnInit, IListable {

  @Output()
  public contactsChanged = new EventEmitter<UserContactRequest>();
  public columns: Column[] = [
    { name: 'Display Name', value: 'DisplayName'},
    { name: 'Date Accepted', value: 'DateAccepted'}
  ];
  public contacts: UserContact[];

  constructor(
    private readonly contactsService: ContactsService
  ) { 
    super();
  }

  ngOnInit(): void {
      this.searchRequest = {
        take: 12,
        skip: 0,
        useNGrams: false,
        sortDirection: 0
      };
      this.loadContacts();
    }

  loadContacts() {
    this.contactsService.findContactRequestsAsync(this.searchRequest).subscribe(
      (searchResult) => {
        this.searchResult = searchResult;
        this.contacts = searchResult.results;
        this.totalCount = searchResult.count;
        this.searchRequest.filters = searchResult.filters;
      }
    );
  }

  acceptContactAsync(userContactRequest: UserContactRequest) {
    this.contactsService.addContact(userContactRequest).subscribe(() => {
      this.loadContacts();
      this.contactsChanged.emit(userContactRequest);
    });
  }

  removeContactRequestAsync(userContactRequest: UserContactRequest) {
    this.contactsService.removeContactRequestAsync(userContactRequest.id).subscribe(() => {
      this.loadContacts();
    });
  }
  
  public search(): void {
    this.loadContacts();
  }
}

