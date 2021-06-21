import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { PageRequest, SortRequest } from '../../models/search-request';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.css']
})
export class PagerComponent implements OnInit, OnChanges {

  @Input()
  public count: number;
  @Input()
  public take: number;
  public skip: number;
  @Output()
  public pageChange = new EventEmitter<PageRequest>();
  public totalPages: number;
  public currentPage = 1;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.count.currentValue !== changes.count.previousValue) {
      this.currentPage = 1;
      this.totalPages = Math.ceil(this.count / this.take);
    }
  }

  public page(pages: number = 0) {
    this.currentPage += pages;
    const skip = (this.currentPage - 1) * this.take;
    this.totalPages = Math.ceil(this.count / this.take);
    this.pageChange.emit( { skip: skip, take: this.take });
  }

  public takeChanged() {
    this.currentPage = 1;
    this.page();
  }
}
