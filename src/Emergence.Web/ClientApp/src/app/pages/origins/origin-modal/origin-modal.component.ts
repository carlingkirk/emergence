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
  public name: string;

  constructor() { }

  ngOnInit(): void {
  }

  originLoad(origin: Origin): void {
    this.name = origin.name;
  }
}
