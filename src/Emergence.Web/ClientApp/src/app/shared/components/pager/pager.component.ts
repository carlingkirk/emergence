import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { PageRequest, SortRequest } from '../../models/search-request';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.css']
})
export class PagerComponent implements OnInit {

  @Input()
  public count: number;
  @Input()
  public take: number;
  public skip: number;
  @Output()
  public pageChange = new EventEmitter<PageRequest>();
  public totalPages: number;
  public currentPage: number = 1;
  public pageDisplay: string;

  constructor() { }

  ngOnInit(): void {
    this.pageDisplay = this.getPageDisplay();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.count.currentValue != changes.count.previousValue) {
      this.currentPage = 1;
      this.totalPages = Math.ceil(this.count / this.take);
    }
  }

  public getPageDisplay() {
    return this.currentPage + " of " + this.totalPages;
  }

  public page(pages: number = 0) {
    this.currentPage += pages;
    let skip = (this.currentPage - 1) * this.take;
    this.totalPages = Math.ceil(this.count / this.take);
    this.pageChange.emit( { skip: skip, take: this.take })
  }

  public takeChanged() {
    this.currentPage = 1;
    this.page();
  }
}
