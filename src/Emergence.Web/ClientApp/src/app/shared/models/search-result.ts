export interface SearchResult {
    filters: Filter[];
    count: number;
    results: [];
}

export interface Filter {
    filter: any;
}