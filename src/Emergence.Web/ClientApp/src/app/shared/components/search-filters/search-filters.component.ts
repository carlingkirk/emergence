import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SpecimenStage } from '../../models/enums';
import { FilterBody, Filter } from '../../models/filters';

@Component({
  selector: 'app-search-filters',
  templateUrl: './search-filters.component.html',
  styleUrls: ['./search-filters.component.css']
})
export class SearchFiltersComponent implements OnInit {
  @Input()
  public filterBody: FilterBody;
  @Output()
  public filtersChanged = new EventEmitter<FilterBody>();
  public filters: Filter[] = [];
  constructor() { }

  ngOnInit(): void {
    if (this.filterBody.stageFilter) {
      this.filterBody.stageFilter.displayValues = [];
      for(let key in this.filterBody.stageFilter.facetValues) {
        let value = this.filterBody.stageFilter.facetValues[key];
        let name = +key as SpecimenStage;
        this.filterBody.stageFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.stageFilter);
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
