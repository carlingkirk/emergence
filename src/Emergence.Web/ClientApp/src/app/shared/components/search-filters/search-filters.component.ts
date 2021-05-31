import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LightType, Month, Region, SpecimenStage, WaterType } from '../../models/enums';
import { FilterBody, Filter, DisplayValue, SelectRangeFilter } from '../../models/filters';
import { getZones } from '../../models/zone';

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
  private zones: { [id: number]: string } = getZones();
  constructor() { }

  ngOnInit(): void {
  }

  filter() {
    this.filters = [];
    if (this.filterBody.stageFilter) {
      this.filterBody.stageFilter.width = 4;
      this.filters = [ this.filterBody.stageFilter ];
    }
    if (this.filterBody.zoneFilter) {
      this.filterBody.zoneFilter.displayValues = [];
      for(let key in this.filterBody.zoneFilter.facetValues) {
        let value = this.filterBody.zoneFilter.facetValues[key];
        let name = this.zones[key];
        this.filterBody.zoneFilter.displayValues.push({ name: name, value: value })
      }
      this.filterBody.zoneFilter.width = 3;
      this.filters.push(this.filterBody.zoneFilter);
    }

    if (this.filterBody.regionFilter) {
      this.filterBody.regionFilter.displayValues = [];
      for(let key in this.filterBody.regionFilter.facetValues) {
        let value = this.filterBody.regionFilter.facetValues[key];
        let name = +key as Region;
        this.filterBody.regionFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filterBody.regionFilter.width = 4;
      this.filters.push(this.filterBody.regionFilter);
    }

    if (this.filterBody.bloomFilter) {
      this.filterBody.bloomFilter = this.mapSelectRangeFilter(this.filterBody.bloomFilter);
      this.filterBody.bloomFilter.width = 4;
      this.filters.push(this.filterBody.bloomFilter);
    }

    if (this.filterBody.heightFilter) {
      this.filterBody.heightFilter = this.mapSelectRangeNumFilter(this.filterBody.heightFilter);
      this.filterBody.heightFilter.width = 4;
      this.filters.push(this.filterBody.heightFilter);
    }

    if (this.filterBody.spreadFilter) {
      this.filterBody.spreadFilter = this.mapSelectRangeNumFilter(this.filterBody.spreadFilter);
      this.filterBody.spreadFilter.width = 4;
      this.filters.push(this.filterBody.spreadFilter);
    }

    if (this.filterBody.lightFilter) {
      this.filterBody.lightFilter = this.mapSelectRangeFilter(this.filterBody.lightFilter);
      this.filterBody.lightFilter.width = 5;
      this.filters.push(this.filterBody.lightFilter);
    }

    if (this.filterBody.waterFilter) {
      this.filterBody.waterFilter = this.mapSelectRangeFilter(this.filterBody.waterFilter);
      this.filterBody.waterFilter.width = 5;
      this.filters.push(this.filterBody.waterFilter);
    }

    if (this.filterBody.nativeFilter) {
      this.filterBody.nativeFilter.displayValues = [];
      for(let key in this.filterBody.nativeFilter.facetValues) {
        let value = this.filterBody.nativeFilter.facetValues[key];
        let name = +key as LightType;
        this.filterBody.nativeFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filterBody.nativeFilter.width = 4;
      this.filters.push(this.filterBody.nativeFilter);
    }
  }
  mapSelectRangeFilter(filter: SelectRangeFilter): SelectRangeFilter {
    filter.minDisplayValues = [];
    filter.maxDisplayValues = [];
    for(let key in filter.facetValues) {
      let value = filter.facetValues[key];
      filter.minDisplayValues.push({ name: key, value: value });
      filter.maxDisplayValues.push({ name: key, value: value })
    }
    return filter;
  }

  mapSelectRangeNumFilter(filter: SelectRangeFilter): SelectRangeFilter {
    filter.minDisplayValues = [];
    filter.maxDisplayValues = [];

    for(let key in filter.minFacetValues) {
      let value = filter.minFacetValues[key];
      filter.minDisplayValues.push({ name: key.toString(), value: value })
    }
    filter.minDisplayValues = this.sortDisplayValues(filter.minDisplayValues);

    for(let key in filter.maxFacetValues) {
      let value = filter.maxFacetValues[key];
      filter.maxDisplayValues.push({ name: key.toString(), value: value })
    }
    filter.maxDisplayValues = this.sortDisplayValues(filter.maxDisplayValues);

    return filter;
  }

  sortDisplayValues(displayValues: DisplayValue[]): DisplayValue[] {
    return displayValues.sort((a, b) => Number(a.name) > Number(b.name) ? 1 : Number(a.name) === Number(b.name) ? 0 : -1);
  }

  valueChanged(): void {
    this.filters.forEach((filter) => {
      if (filter.name === "Stage") {
        this.filterBody.stageFilter.displayValues = null;
        this.filterBody.stageFilter.value = filter.value;
      }
      if (filter.name === "Zone") {
        this.filterBody.zoneFilter.displayValues = null;
        this.filterBody.zoneFilter.value = filter.value;
      }
      if (filter.name === "Region") {
        this.filterBody.regionFilter.displayValues = null;
        this.filterBody.regionFilter.value = filter.value;
      }
      if (filter.name === "Bloom") {
        this.filterBody.bloomFilter.displayValues = null;
        this.filterBody.bloomFilter.value = filter.value;
      }
      if (filter.name === "Height") {
        this.filterBody.heightFilter.minDisplayValues = null;
        this.filterBody.heightFilter.maxDisplayValues = null;
        this.filterBody.heightFilter.value = filter.value;
      }
      if (filter.name === "Spread") {
        this.filterBody.spreadFilter.minDisplayValues = null;
        this.filterBody.spreadFilter.maxDisplayValues = null;
        this.filterBody.spreadFilter.value = filter.value;
      }
      if (filter.name === "Light") {
        this.filterBody.lightFilter.minDisplayValues = null;
        this.filterBody.lightFilter.maxDisplayValues = null;
        this.filterBody.lightFilter.value = filter.value;
      }
      if (filter.name === "Water") {
        this.filterBody.waterFilter.minDisplayValues = null;
        this.filterBody.waterFilter.maxDisplayValues = null;
        this.filterBody.waterFilter.value = filter.value;
      }
      if (filter.name === "Native") {
        this.filterBody.nativeFilter.displayValues = null;
        this.filterBody.nativeFilter.value = filter.value;
      }
    });
    this.filtersChanged.emit(this.filterBody);
  }
}
