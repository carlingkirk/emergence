import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SpecimenService } from 'src/app/service/specimen-service';
import { InventoryItemStatus, ItemType, SpecimenStage, Visibility } from 'src/app/shared/models/enums';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-edit',
  templateUrl: './specimen-edit.component.html',
  styleUrls: ['./specimen-edit.component.css']
})
export class SpecimenEditComponent implements OnInit {
  @Input()
  public specimen: Specimen;
  @Input()
  public id: number;
  public inventoryItemStatuses: InventoryItemStatus[];
  public visibilities: Visibility[];
  public specimenStages: SpecimenStage[];
  public itemTypes: ItemType[];
  public modal: boolean;
  private inventoryItemStatusEnum = InventoryItemStatus;

  constructor(private readonly specimenService: SpecimenService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];

    this.inventoryItemStatuses = Object.keys(InventoryItemStatus).filter(key => !isNaN(Number(key))).map(key => InventoryItemStatus[key]);
    this.visibilities = Object.keys(Visibility).filter(key => !isNaN(Number(key))).map(key => Visibility[key]);
    this.specimenStages = Object.keys(SpecimenStage).filter(key => !isNaN(Number(key))).map(key => SpecimenStage[key]);
    this.itemTypes = Object.keys(ItemType).filter(key => !isNaN(Number(key))).map((key) => ItemType[key]);

    if (!this.specimen) {
      this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
        this.specimen = specimen;
      });
    }
  }

  public saveSpecimen(): void {

  }

  public populateInventoryItemName(): void {

  }

  public cancel(): void {

  }

  public closeModal(): void {
    
  }
}
