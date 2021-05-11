import { Filter, FilterResponse } from "./filters";

export class SearchRequest {
    filters?: Filter[];
    shape?: any;
    useNGrams: boolean;
    searchText?: string;
    skip: number;
    take: number;
    sortBy?: string;
    sortDirection?: string;
    createdBy?: string;
}