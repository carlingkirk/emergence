import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SpecimenStage } from '../../models/enums';
import { FilterResponse, Filter } from '../../models/filters';

@Component({
  selector: 'app-search-filters',
  templateUrl: './search-filters.component.html',
  styleUrls: ['./search-filters.component.css']
})
export class SearchFiltersComponent implements OnInit {
  @Input()
  public filterResponse: FilterResponse;
  @Output()
  public outFilters = new EventEmitter<Filter[]>();
  public filters: Filter[] = [];
  constructor() { }

  ngOnInit(): void {
    if (this.filterResponse.stageFilter) {
      this.filterResponse.stageFilter.displayValues = [];
      for(let key in this.filterResponse.stageFilter.facetValues) {
        let value = this.filterResponse.stageFilter.facetValues[key];
        let name = +key as SpecimenStage;
        this.filterResponse.stageFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterResponse.stageFilter);
    }
  }

  valueChanged(): void {
    this.outFilters.emit(this.filters);
  }
}
