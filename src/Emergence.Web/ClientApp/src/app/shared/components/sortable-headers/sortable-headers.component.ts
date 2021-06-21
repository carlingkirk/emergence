import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SortDirection } from '../../models/enums';
import { SortRequest } from '../../models/search-request';

@Component({
  selector: 'app-sortable-headers',
  templateUrl: './sortable-headers.component.html',
  styleUrls: ['./sortable-headers.component.css']
})
export class SortableHeadersComponent implements OnInit {
  @Input()
  public columns: Column[];
  @Input()
  public sortBy: string;
  @Input()
  public sortDirection: SortDirection;
  @Output()
  public sortChanged = new EventEmitter<SortRequest>();
  constructor() {}

  ngOnInit(): void {
  }

  sort(column: string) {
    if (this.sortDirection !== SortDirection.Ascending) {
      this.sortDirection = SortDirection.Ascending;
    } else {
      this.sortDirection = SortDirection.Descending;
    }
    this.sortBy = column;
    this.sortChanged.emit({ sortBy: this.sortBy, sortDirection: this.sortDirection});
  }
}

export interface Column {
  name: string;
  value: string;
}
