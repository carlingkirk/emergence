import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LightType, Month, Region, SpecimenStage, WaterType } from '../../models/enums';
import { FilterBody, Filter } from '../../models/filters';
import { BloomTime } from '../../models/plant-info';

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
      this.filters = [ this.filterBody.stageFilter ];
    }
    if (this.filterBody.zoneFilter) {
      this.filterBody.zoneFilter.displayValues = [];
      for(let key in this.filterBody.zoneFilter.facetValues) {
        let value = this.filterBody.zoneFilter.facetValues[key];
        let name = +key as number;
        this.filterBody.zoneFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.zoneFilter);
    }
    if (this.filterBody.regionFilter) {
      this.filterBody.regionFilter.displayValues = [];
      for(let key in this.filterBody.regionFilter.facetValues) {
        let value = this.filterBody.regionFilter.facetValues[key];
        let name = +key as Region;
        this.filterBody.regionFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.regionFilter);
    }
    if (this.filterBody.bloomFilter) {
      this.filterBody.bloomFilter.displayValues = [];
      for(let key in this.filterBody.bloomFilter.facetValues) {
        let value = this.filterBody.bloomFilter.facetValues[key];
        let name = +key as Month;
        this.filterBody.bloomFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.bloomFilter);
    }
    if (this.filterBody.heightFilter) {
      this.filterBody.heightFilter.displayValues = [];
      for(let key in this.filterBody.heightFilter.facetValues) {
        let value = this.filterBody.heightFilter.facetValues[key];
        let name = +key as number;
        this.filterBody.heightFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.heightFilter);
    }
    if (this.filterBody.lightFilter) {
      this.filterBody.lightFilter.displayValues = [];
      for(let key in this.filterBody.lightFilter.facetValues) {
        let value = this.filterBody.lightFilter.facetValues[key];
        let name = +key as LightType;
        this.filterBody.lightFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.lightFilter);
    }
    if (this.filterBody.nativeFilter) {
      this.filterBody.nativeFilter.displayValues = [];
      for(let key in this.filterBody.nativeFilter.facetValues) {
        let value = this.filterBody.nativeFilter.facetValues[key];
        let name = +key as LightType;
        this.filterBody.nativeFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.nativeFilter);
    }
    if (this.filterBody.spreadFilter) {
      this.filterBody.spreadFilter.displayValues = [];
      for(let key in this.filterBody.spreadFilter.facetValues) {
        let value = this.filterBody.spreadFilter.facetValues[key];
        let name = +key as number;
        this.filterBody.spreadFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.spreadFilter);
    }
    if (this.filterBody.waterFilter) {
      this.filterBody.waterFilter.displayValues = [];
      for(let key in this.filterBody.waterFilter.facetValues) {
        let value = this.filterBody.waterFilter.facetValues[key];
        let name = +key as WaterType;
        this.filterBody.waterFilter.displayValues.push({ name: name[key], value: value })
      }
      this.filters.push(this.filterBody.waterFilter);
    }
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
        this.filterBody.heightFilter.displayValues = null;
        this.filterBody.heightFilter.value = filter.value;
      }
      if (filter.name === "Spread") {
        this.filterBody.spreadFilter.displayValues = null;
        this.filterBody.spreadFilter.value = filter.value;
      }
      if (filter.name === "Light") {
        this.filterBody.lightFilter.displayValues = null;
        this.filterBody.lightFilter.value = filter.value;
      }
      if (filter.name === "Water") {
        this.filterBody.waterFilter.displayValues = null;
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
