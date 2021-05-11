export class Filter {
    name: string;
    value: any;
    filterType: number;
    inputType: number;
}

export class FilterResponse {
    stageFilter: SelectFilter;
    zoneFilter: SelectRangeFilter;
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