import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FilterBody, Filter, DisplayValue, SelectRangeFilter, SelectFilter } from '../../models/filters';

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
    this.filters = [];
    if (this.filterBody.stageFilter) {
      this.filterBody.stageFilter = this.mapSelectFilter(this.filterBody.stageFilter);
      this.filterBody.stageFilter.width = 4;
      this.filters = [ this.filterBody.stageFilter ];
    }
    if (this.filterBody.zoneFilter) {
      this.filterBody.zoneFilter = this.mapSelectFilter(this.filterBody.zoneFilter);
      this.filterBody.zoneFilter.width = 2;
      this.filterBody.zoneFilter.sort = 1;
      this.filterBody.zoneFilter.displayValues.sort((a, b) => a.name > b.name ? 1 : a.name === b.name ? 0 : -1);
      this.filters.push(this.filterBody.zoneFilter);
    }

    if (this.filterBody.regionFilter) {
      this.filterBody.regionFilter = this.mapSelectFilter(this.filterBody.regionFilter);
      this.filterBody.regionFilter.width = 4;
      this.filterBody.regionFilter.sort = 2;
      this.filters.push(this.filterBody.regionFilter);
    }

    if (this.filterBody.bloomFilter) {
      this.filterBody.bloomFilter = this.mapSelectRangeFilter(this.filterBody.bloomFilter);
      this.filterBody.bloomFilter.width = 3;
      this.filterBody.bloomFilter.sort = 3;
      this.filters.push(this.filterBody.bloomFilter);
    }

    if (this.filterBody.nativeFilter) {
      this.filterBody.nativeFilter = this.mapSelectFilter(this.filterBody.nativeFilter);
      this.filterBody.nativeFilter.width = 2;
      this.filterBody.nativeFilter.sort = 4;
      this.filters.push(this.filterBody.nativeFilter);
    }

    if (this.filterBody.heightFilter) {
      this.filterBody.heightFilter = this.mapSelectRangeNumFilter(this.filterBody.heightFilter);
      this.filterBody.heightFilter.width = 2;
      this.filterBody.heightFilter.sort = 5;
      this.filters.push(this.filterBody.heightFilter);
    }

    if (this.filterBody.spreadFilter) {
      this.filterBody.spreadFilter = this.mapSelectRangeNumFilter(this.filterBody.spreadFilter);
      this.filterBody.spreadFilter.width = 2;
      this.filterBody.spreadFilter.sort = 6;
      this.filters.push(this.filterBody.spreadFilter);
    }

    if (this.filterBody.lightFilter) {
      this.filterBody.lightFilter = this.mapSelectRangeFilter(this.filterBody.lightFilter);
      this.filterBody.lightFilter.width = 4;
      this.filterBody.lightFilter.sort = 7;
      this.filters.push(this.filterBody.lightFilter);
    }

    if (this.filterBody.waterFilter) {
      this.filterBody.waterFilter = this.mapSelectRangeFilter(this.filterBody.waterFilter);
      this.filterBody.waterFilter.width = 4;
      this.filterBody.waterFilter.sort = 8;
      this.filters.push(this.filterBody.waterFilter);
    }
    this.filters.sort((a, b) => a.sort > b.sort ? 1 : a.sort === b.sort ? 0 : -1);
  }

  mapSelectFilter(filter: SelectFilter): SelectFilter {
    filter.displayValues = [];
    for (const key of Object.keys(filter.facetValues)) {
      const value = filter.facetValues[key];
      filter.displayValues.push({ name: key, value: value, key: key });
    }
    return filter;
  }

  mapSelectRangeFilter(filter: SelectRangeFilter): SelectRangeFilter {
    filter.minDisplayValues = [];
    filter.maxDisplayValues = [];
    for (const key of Object.keys(filter.facetValues)) {
      const value = filter.facetValues[key];
      filter.minDisplayValues.push({ name: key, value: value, key: key });
      filter.maxDisplayValues.push({ name: key, value: value, key: key });
    }
    return filter;
  }

  mapSelectRangeNumFilter(filter: SelectRangeFilter): SelectRangeFilter {
    filter.minDisplayValues = [];
    filter.maxDisplayValues = [];

    for (const key of Object.keys(filter.minFacetValues)) {
      const value = filter.minFacetValues[key];
      filter.minDisplayValues.push({ name: key, value: value, key: key });
    }
    filter.minDisplayValues = this.sortDisplayNumValues(filter.minDisplayValues);

    for (const key of Object.keys(filter.maxFacetValues)) {
      const value = filter.maxFacetValues[key];
      filter.maxDisplayValues.push({ name: key, value: value, key: key });
    }
    filter.maxDisplayValues = this.sortDisplayNumValues(filter.maxDisplayValues);

    return filter;
  }

  sortDisplayNumValues(displayValues: DisplayValue[]): DisplayValue[] {
    return displayValues.sort((a, b) => Number(a.name) > Number(b.name) ? 1 : Number(a.name) === Number(b.name) ? 0 : -1);
  }

  valueChanged(): void {
    this.filters.forEach((filter) => {
      if (filter.name === 'Stage') {
        this.filterBody.stageFilter.displayValues = null;
        this.filterBody.stageFilter.value = filter.value;
      }
      if (filter.name === 'Zone') {
        this.filterBody.zoneFilter.displayValues = null;
        this.filterBody.zoneFilter.value = filter.value;
      }
      if (filter.name === 'Region') {
        this.filterBody.regionFilter.displayValues = null;
        this.filterBody.regionFilter.value = filter.value;
      }
      if (filter.name === 'Bloom') {
        this.filterBody.bloomFilter.displayValues = null;
        this.filterBody.bloomFilter.value = filter.value;
      }
      if (filter.name === 'Height') {
        this.filterBody.heightFilter.minDisplayValues = null;
        this.filterBody.heightFilter.maxDisplayValues = null;
        this.filterBody.heightFilter.value = filter.value;
      }
      if (filter.name === 'Spread') {
        this.filterBody.spreadFilter.minDisplayValues = null;
        this.filterBody.spreadFilter.maxDisplayValues = null;
        this.filterBody.spreadFilter.value = filter.value;
      }
      if (filter.name === 'Light') {
        this.filterBody.lightFilter.minDisplayValues = null;
        this.filterBody.lightFilter.maxDisplayValues = null;
        this.filterBody.lightFilter.value = filter.value;
      }
      if (filter.name === 'Water') {
        this.filterBody.waterFilter.minDisplayValues = null;
        this.filterBody.waterFilter.maxDisplayValues = null;
        this.filterBody.waterFilter.value = filter.value;
      }
      if (filter.name === 'Native') {
        this.filterBody.nativeFilter.displayValues = null;
        this.filterBody.nativeFilter.value = filter.value;
      }
    });
    this.filtersChanged.emit(this.filterBody);
  }
}
