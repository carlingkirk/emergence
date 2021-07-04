import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Origin } from 'src/app/shared/models/origin';

@Component({
  selector: 'app-origin-page',
  templateUrl: './origin-page.component.html',
  styleUrls: ['./origin-page.component.css']
})
export class OriginPageComponent implements OnInit {

  @Input()
  public origin: Origin;

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
  }

  hasAddressInfo() {
    if (this.origin.location) {
      return this.origin.location.addressLine1 || 
        this.origin.location.addressLine2 || 
        this.origin.location.city || 
        this.origin.location.stateOrProvince || 
        this.origin.location.country || 
        this.origin.location.postalCode ||
        this.origin.location.latLong;
    }
  }

  showOriginModal(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title', size: 'lg'});
  }
}
