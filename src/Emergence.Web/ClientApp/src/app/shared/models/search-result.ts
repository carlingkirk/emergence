import { Filter, FilterResponse } from "./filters";

export interface SearchResult {
    filters: Filter[];
    count: number;
    results: [];
}