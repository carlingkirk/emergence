import { FilterBody } from "./filters";

export class SearchRequest {
    filters?: FilterBody;
    shape?: any;
    useNGrams: boolean;
    searchText?: string;
    skip: number;
    take: number;
    sortBy?: string;
    sortDirection?: number;
    createdBy?: string;
}

export class SortRequest {
    sortBy?: string;
    sortDirection?: number;
}

export class PageRequest {
    skip: number;
    take: number;
}