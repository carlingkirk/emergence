import { Filter, FilterResponse } from "./filters";

export class SearchResult {
    filters: Filter[];
    count: number;
    results: [];
}