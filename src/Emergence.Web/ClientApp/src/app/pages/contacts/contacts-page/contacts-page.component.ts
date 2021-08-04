import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contacts-page',
  templateUrl: './contacts-page.component.html',
  styleUrls: ['./contacts-page.component.css']
})
export class ContactsPageComponent implements OnInit {

  public tabs: any = [
    { key: 'contacts', value: 'Contacts'},
    { key: 'requests', value: 'Requests'},
    { key: 'inbox', value: 'Inbox'},
    { key: 'sent', value: 'Sent'}
  ];
  public currentTab = 'contacts';

  constructor() { }

  ngOnInit(): void {
  }

  public switchTab(tab: string) {
    this.currentTab = tab;
  }
}
