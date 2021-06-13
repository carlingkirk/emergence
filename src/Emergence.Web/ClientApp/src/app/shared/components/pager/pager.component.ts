import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  @Output()
  public pageChange = new EventEmitter<PageRequest>();
  public totalPages: number;
  public currentPage: number = 1;

  constructor() { }

  ngOnInit(): void {
  }

  public getPageDisplay() {
    return this.currentPage + " of " + Math.floor(this.count / this.take);
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
