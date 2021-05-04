import { Component, Input, OnInit } from '@angular/core';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-edit',
  templateUrl: './specimen-edit.component.html',
  styleUrls: ['./specimen-edit.component.css']
})
export class SpecimenEditComponent implements OnInit {
  @Input()
  public specimen: Specimen;
  
  constructor() { }

  ngOnInit(): void {
  }

}
