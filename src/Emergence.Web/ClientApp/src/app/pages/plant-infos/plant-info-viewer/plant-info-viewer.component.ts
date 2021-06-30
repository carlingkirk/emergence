import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { PlantInfoService } from 'src/app/service/plant-info-service';
import { Viewer } from 'src/app/shared/interface/viewer';
import { PlantInfo } from 'src/app/shared/models/plant-info';

@Component({
  selector: 'app-plant-info-viewer',
  templateUrl: './plant-info-viewer.component.html',
  styleUrls: ['./plant-info-viewer.component.css']
})
export class PlantInfoViewerComponent extends Viewer {
  @Input()
  public id: number;
  public plantInfo: PlantInfo;
  public commonName: string;
  public scientificName: string;
  public isAuthenticated: Observable<boolean>;
  
  constructor(
    authorizeService: AuthorizeService,
    private readonly plantInfoService: PlantInfoService,
    route: ActivatedRoute,
    private router: Router) {
      super(authorizeService, route);
  }

  ngOnInit(): void {
    super.ngOnInit();
    
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
