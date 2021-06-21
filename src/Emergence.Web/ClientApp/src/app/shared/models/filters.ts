export class Filter {
    name: string;
    value: any;
    filterType: number;
    inputType: number;
    width: number;
    sort ? = 0;
}

export class FilterBody {
    stageFilter: SelectFilter;
    zoneFilter: SelectFilter;
    regionFilter: SelectFilter;
    bloomFilter: SelectRangeFilter;
    heightFilter: SelectRangeFilter;
    spreadFilter: SelectRangeFilter;
    lightFilter: SelectRangeFilter;
    waterFilter: SelectRangeFilter;
    nativeFilter: SelectFilter;
}

export class SelectFilter extends Filter {
    facetValues: { [key: string]: number; };
    displayValues: DisplayValue[];
}

export class SelectRangeFilter extends SelectFilter {
    minimumValue: any;
    maximumValue: any;
    minFacetValues: { [key: string]: number; };
    maxFacetValues: { [key: string]: number; };
    minDisplayValues: DisplayValue[];
    maxDisplayValues: DisplayValue[];
}

export class DisplayValue {
    name: string;
    value: string | number;
    key: string;
}
