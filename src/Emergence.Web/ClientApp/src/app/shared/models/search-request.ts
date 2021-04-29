import { FilterResponse } from "./filters";

export interface SearchRequest {
    filters?: FilterResponse,
    shape?: any;
    useNGrams: boolean;
    searchText?: string;
    skip: number;
    take: number;
    sortBy?: string;
    sortDirection?: string;
    createdBy?: string;
}