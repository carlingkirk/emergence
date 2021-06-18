import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  constructor(private modalService: NgbModal) {
    this.displayName = this.activity?.specimen?.lifeform?.commonName ?? this.activity?.specimen?.name;
   }

  ngOnInit(): void {
  }

  showSpecimenModal(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title', size: 'lg'});
  }
}
