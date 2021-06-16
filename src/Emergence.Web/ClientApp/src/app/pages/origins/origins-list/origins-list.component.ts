import { Component, Input, OnInit } from '@angular/core';
import { OriginService } from 'src/app/service/origin-service';
import { StorageService } from 'src/app/service/storage-service';
import { Column } from 'src/app/shared/components/sortable-headers/sortable-headers.component';
import { IListable, Listable } from 'src/app/shared/interface/list';
import { Origin } from 'src/app/shared/models/origin';

@Component({
  selector: 'app-origins-list',
  templateUrl: './origins-list.component.html',
  styleUrls: ['./origins-list.component.css']
})
export class OriginsListComponent extends Listable implements OnInit, IListable {

  @Input()
  public showSearch: boolean = true;
  public origins: Origin[];
  constructor(
    private readonly originService: OriginService,
    private readonly storageService: StorageService
    ) {
    super();
  }

  public columns: Column[] = [
    { name: 'Name', value: 'Name'},
    { name: 'Type', value: 'Type'},
    { name: 'Description', value: 'Description'},
    { name: 'Parent Origin', value: 'ParentOrigin'},
    { name: 'City', value: 'City'},
    { name: 'Link', value: 'Link'}
  ];

  ngOnInit(): void {
    this.storageService.getItem("origin-search").then((searchRequest) => {
      if (!searchRequest) {
        this.resetSearch();
      } else {
        this.searchRequest = searchRequest;
      }
      this.loadOrigins();
    });
  }

  loadOrigins() {
    this.storageService.setItem("origin-search", this.searchRequest).then((result) => {
      this.originService.findOrigins(this.searchRequest).subscribe(
        (searchResult) => {
          this.searchResult = searchResult;
          this.origins = searchResult.results;
          this.totalCount = searchResult.count;

          this.searchRequest.filters = searchResult.filters;
        }
      );
    });
  }

  public search(): void {
    this.loadOrigins();
  }
}
