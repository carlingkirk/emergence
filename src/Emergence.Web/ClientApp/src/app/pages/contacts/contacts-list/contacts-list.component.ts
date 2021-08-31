import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactsService } from 'src/app/service/contacts-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { UserContact } from 'src/app/shared/models/user-contacts';

@Component({
  selector: 'app-contacts-list',
  templateUrl: './contacts-list.component.html',
  styleUrls: ['./contacts-list.component.css']
})
export class ContactsListComponent extends Listable implements OnInit, IListable {

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
    this.contactsService.findContactsAsync(this.searchRequest).subscribe(
      (searchResult) => {
        this.searchResult = searchResult;
        this.contacts = searchResult.results;
        this.totalCount = searchResult.count;
        this.searchRequest.filters = searchResult.filters;
      }
    );
  }

  removeContactAsync(id: number) {
    this.contactsService.removeContactAsync(id).subscribe(() => {
      this.loadContacts();
    });
  }
  
  public search(): void {
    this.loadContacts();
  }
}
