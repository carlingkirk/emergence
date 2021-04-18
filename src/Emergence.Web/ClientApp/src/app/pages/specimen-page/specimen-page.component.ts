import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SpecimenService } from 'src/app/service/specimen-service';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-page',
  templateUrl: './specimen-page.component.html',
  styleUrls: ['./specimen-page.component.css']
})
export class SpecimenPageComponent implements OnInit {
  @Input()
  public id: number;
  public specimen: Specimen;

  constructor(
    private readonly specimenService: SpecimenService,
    private route: ActivatedRoute
  ) { 
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    console.log("Id: " + this.id);
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => 
        this.specimen = specimen
    );
  }
}
