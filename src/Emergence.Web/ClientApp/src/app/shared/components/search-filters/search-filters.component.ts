import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { SpecimenStage } from '../../models/enums';
import { FilterBody, Filter } from '../../models/filters';

@Component({
  selector: 'app-search-filters',
  templateUrl: './search-filters.component.html',
  styleUrls: ['./search-filters.component.css']
})
export class SearchFiltersComponent implements OnInit {
  private _filterBody: FilterBody;
  @Input() set filterBody(value: FilterBody) {
    this._filterBody = value;
    this.filter();
  }
  get filterBody(): FilterBody {
      return this._filterBody;
  }
  @Output()
  public filtersChanged = new EventEmitter<FilterBody>();
  public filters: Filter[] = [];
  constructor() { }

  ngOnInit(): void {
  }

  filter() {
    if (this.filterBody.stageFilter) {
      this.filterBody.stageFilter.displayValues = [];
      for(let key in this.filterBody.stageFilter.facetValues) {
        let value = this.filterBody.stageFilter.facetValues[key];
        let name = +key as SpecimenStage;
        this.filterBody.stageFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters =[ this.filterBody.stageFilter ];
    }
  }

  valueChanged(): void {
    this.filters.forEach((filter) => {
      if (filter.name === "Stage") {
        this.filterBody.stageFilter.displayValues = null;
        this.filterBody.stageFilter.value = filter.value;
      }
    });
    this.filtersChanged.emit(this.filterBody);
  }
}
