import { Component, Input, OnInit } from '@angular/core';
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
  public specimenId: number;
  @Input()
  public modal: NgbModalRef;
  public name: string;
  public scientificName: string;

  constructor() { }

  ngOnInit(): void {
  }

  specimenLoad(specimen: Specimen): void {
    this.name = getSpecimenName(specimen);
    this.scientificName = getSpecimenScientificName(specimen);
  }
}
