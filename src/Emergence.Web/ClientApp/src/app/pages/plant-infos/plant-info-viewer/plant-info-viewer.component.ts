import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
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
  public isOwner = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly plantInfoService: PlantInfoService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user['sub'];

      this.plantInfoService.getPlantInfo(this.id).subscribe((plantInfo) => {
        this.plantInfo = plantInfo;
        this.scientificName = plantInfo.scientificName ?? plantInfo.lifeform.scientificName;
        this.commonName = plantInfo.commonName ?? plantInfo.lifeform.commonName;
        this.isOwner = this.plantInfo.createdBy === this.user.userId;
        return of({});
      });
      return of({});
    });
  }

  public removePlantInfo() {
    this.plantInfoService.deletePlantInfo(this.id).subscribe(() => {
      this.router.navigate(['/plantinfos/list']);
    });
  }
}
