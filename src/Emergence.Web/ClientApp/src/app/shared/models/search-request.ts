import { FilterBody } from "./filters";

export class SearchRequest {
    filters?: FilterBody;
    shape?: any;
    useNGrams: boolean;
    searchText?: string;
    skip: number;
    take: number;
    sortBy?: string;
    sortDirection?: string;
    createdBy?: string;
}