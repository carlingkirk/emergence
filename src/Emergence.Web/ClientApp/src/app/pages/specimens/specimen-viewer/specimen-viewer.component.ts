import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService, IUser } from 'src/api-authorization/authorize.service';
import { SpecimenService } from 'src/app/service/specimen-service';
import { Specimen } from 'src/app/shared/models/specimen';

@Component({
  selector: 'app-specimen-viewer',
  templateUrl: './specimen-viewer.component.html',
  styleUrls: ['./specimen-viewer.component.css']
})
export class SpecimenViewerComponent implements OnInit {
  @Input()
  public id: number;
  public specimen: Specimen;
  public tabs: any = [ 
    { key: 'specimen', value: 'Specimen'},
    { key: 'activities', value: 'Activities'},
    { key: 'plant-infos', value: 'Plant Profiles'}
  ];
  public currentTab: string = 'specimen';
  public isEditing: boolean = false;
  public isOwner: boolean = false;
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public user: IUser;
  constructor(
    private authorizeService: AuthorizeService,
    private readonly specimenService: SpecimenService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.authorizeService.getUser().subscribe((user) => {
      this.user = user;
      this.user.userId = user["sub"];

      this.specimenService.getSpecimen(this.id).subscribe((specimen) => {
        this.specimen = specimen;
        this.isOwner = this.specimen.createdBy == this.user.userId;
      });
    });
  }

  public switchTab(tab: string) {
    this.currentTab = tab;
  }

  public goBack() {
  }

  public showMessageModal() {
  }

  public removeSpecimen() {
    this.specimenService.deleteSpecimen(this.id).subscribe(() => {
      this.router.navigate(['/specimens/list']);
    });
  }

  public editSpecimen() {
    this.isEditing = true;
  }
}
