export class Filter {
    name: string;
    value: any;
    filterType: number;
    inputType: number;
}

export class FilterBody {
    stageFilter: SelectFilter;
    zoneFilter: SelectRangeFilter;
    regionFilter: SelectFilter;
    bloomFilter: SelectRangeFilter;
    heightFilter: SelectRangeFilter;
    spreadFilter: SelectRangeFilter;
    lightFilter: SelectRangeFilter;
    waterFilter: SelectRangeFilter;
    nativeFilter: SelectFilter;
}

export class SelectFilter implements Filter {
    name: string;
    value: any;
    facetValues: { [key: string]: number; }
    displayValues: DisplayValue[];
    filterType: number;
    inputType: number;
}

export class SelectRangeFilter implements Filter {
    name: string;
    value: any;
    minimumValue: any;
    maximumValue: any;
    facetValues: { [key: string]: number; }
    displayValues: DisplayValue[];
    filterType: number;
    inputType: number;
}

export interface DisplayValue {
    name: string;
    value: string | number;
}