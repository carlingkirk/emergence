import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.css']
})
export class PrivacyComponent implements OnInit {

  constructor() { }

  public emailAddress: string = "carling@emergence.app";
  public mailTo: string = "mailto:" + this.emailAddress;

  ngOnInit(): void {
  }

}
