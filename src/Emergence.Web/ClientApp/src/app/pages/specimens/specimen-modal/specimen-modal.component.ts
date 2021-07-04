import { Component, Input, OnInit, Output } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { getSpecimenName, getSpecimenScientificName } from 'src/app/shared/common';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-modal',
  templateUrl: './specimen-modal.component.html',
  styleUrls: ['./specimen-modal.component.css']
})
export class SpecimenModalComponent implements OnInit {

  @Input()
  public modal: NgbModalRef;
  @Input()
  public specimen: Specimen;
  @Input()
  public isEditing: boolean;
  public name: string;
  public scientificName: string;

  constructor() { }

  ngOnInit(): void {
    this.name = getSpecimenName(this.specimen);
    this.scientificName = getSpecimenScientificName(this.specimen);
  }
}
