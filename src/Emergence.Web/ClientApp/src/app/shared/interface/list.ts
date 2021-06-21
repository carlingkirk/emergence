import { Column } from '../components/sortable-headers/sortable-headers.component';
import { FilterBody } from '../models/filters';
import { PageRequest, SearchRequest, SortRequest } from '../models/search-request';
import { SearchResult } from '../models/search-result';

export interface IListable {
    searchRequest: SearchRequest;
    searchResult: SearchResult;
    totalCount: number ;
    forUserId?: string;
    showSearch: boolean;
    searchText: string;
    showPublic: boolean;
    listView: boolean;
    showFilters: boolean;
    columns: Column[];

    resetSearch(): void;
    toggleFilters(): void;
    search(): void;
    filtersChange(filterBody: FilterBody): void;
    pageChanged(page: PageRequest): void;
    sortChange(sort: SortRequest): void;
}

export class Listable implements IListable {
    public searchRequest: SearchRequest;
    public searchResult: SearchResult;
    public totalCount: number;
    public forUserId?: string;
    public showSearch: boolean;
    public searchText: string;
    public showPublic: boolean;
    public listView: boolean;
    public showFilters: boolean;
    public columns: Column[];

    public resetSearch(): void {
        this.searchRequest = {
            filters: null,
            take: 12,
            skip: 0,
            useNGrams: false,
            sortDirection: 0,
            sortBy: null
        };
        this.search();
    }

    public toggleFilters(): void {
        this.showFilters = !this.showFilters;
    }

    public search(): void {
    }

    public searchTerm(): void {
        this.searchRequest.skip = 0;
        this.search();
    }

    public filtersChange(filterBody: FilterBody): void {
        this.searchRequest.filters = filterBody;

        this.search();
    }

    public pageChanged(page: PageRequest): void {
        this.searchRequest.take = page.take;
        this.searchRequest.skip = page.skip;

        this.search();
    }

    public sortChange(sort: SortRequest): void {
        this.searchRequest.sortBy = sort.sortBy;
        this.searchRequest.sortDirection = sort.sortDirection;

        this.search();
    }
}
