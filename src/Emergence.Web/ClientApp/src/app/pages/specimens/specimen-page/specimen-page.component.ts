import { Component, Input, OnInit } from '@angular/core';
import { SpecimenService } from 'src/app/service/specimen-service';
import { Photo } from 'src/app/shared/models/photo';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-page',
  templateUrl: './specimen-page.component.html',
  styleUrls: ['./specimen-page.component.css']
})
export class SpecimenPageComponent implements OnInit {
  
  @Input()
  public specimen: Specimen;
  public uploadedPhotos: Photo[];

  constructor() { }

  ngOnInit(): void {
  }
}
