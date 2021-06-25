import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { PlantInfoService } from 'src/app/service/plant-info-service';
import { PlantInfo } from 'src/app/shared/models/plant-info';

@Component({
  selector: 'app-plant-info-viewer',
  templateUrl: './plant-info-viewer.component.html',
  styleUrls: ['./plant-info-viewer.component.css']
})
export class PlantInfoViewerComponent implements OnInit {
  @Input()
  public id: number;
  public plantInfo: PlantInfo;
  public commonName: string;
  public scientificName: string;
  public isEditing = false;
  public isAuthenticated: Observable<boolean>;
  public userId: Observable<string>;
  public isOwner: boolean;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly plantInfoService: PlantInfoService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.userId = this.authorizeService.getUserId().pipe(
      tap((u) => { this.isOwner = this.plantInfo.createdBy === u; })
    );
    
    this.plantInfoService.getPlantInfo(this.id).subscribe((plantInfo) => {
      this.plantInfo = plantInfo;
      this.scientificName = plantInfo.scientificName ?? plantInfo.lifeform.scientificName;
      this.commonName = plantInfo.commonName ?? plantInfo.lifeform.commonName;
      return of({});
    });;
  }

  public removePlantInfo() {
    this.plantInfoService.deletePlantInfo(this.id).subscribe(() => {
      this.router.navigate(['/plantinfos/list']);
    });
  }
}
