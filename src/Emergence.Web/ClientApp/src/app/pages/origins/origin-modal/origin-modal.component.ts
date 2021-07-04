import { Component, Input, OnInit } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Origin } from 'src/app/shared/models/origin';

@Component({
  selector: 'app-origin-modal',
  templateUrl: './origin-modal.component.html',
  styleUrls: ['./origin-modal.component.css']
})
export class OriginModalComponent implements OnInit {
  @Input()
  public originId: number;
  @Input()
  public modal: NgbModalRef;
  @Input()
  public origin: Origin;
  @Input()
  public isEditing: boolean;
  public name: string;

  constructor() { }

  ngOnInit(): void {
    this.name = this.origin.name;
  }
}
