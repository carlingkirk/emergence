import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: '[app-sortable-headers]',
  templateUrl: './sortable-headers.component.html',
  styleUrls: ['./sortable-headers.component.css']
})
export class SortableHeadersComponent implements OnInit {
  @Input()
  public columns: Column[];
  constructor() {}

  ngOnInit(): void {
  }

}

export interface Column {
  name: string;
  value: string;
}