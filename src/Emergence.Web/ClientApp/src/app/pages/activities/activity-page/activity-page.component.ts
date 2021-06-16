import { Component, Input, OnInit } from '@angular/core';
import { Activity } from 'src/app/shared/models/activity';

@Component({
  selector: 'app-activity-page',
  templateUrl: './activity-page.component.html',
  styleUrls: ['./activity-page.component.css']
})
export class ActivityPageComponent implements OnInit {

  @Input()
  public activity: Activity;
  public displayName: string;
  constructor() {
    this.displayName = this.activity?.specimen?.lifeform?.commonName ?? this.activity?.specimen?.name;
   }

  ngOnInit(): void {
  }

  showSpecimenModal() {
    // TODO
  }
}
