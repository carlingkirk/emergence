import { FilterResponse } from "./filters";

export interface SearchResult {
    filters: FilterResponse;
    count: number;
    results: [];
}