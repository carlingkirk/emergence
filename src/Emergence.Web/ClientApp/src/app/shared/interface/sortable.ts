import { Column } from "../components/sortable-headers/sortable-headers.component";

export interface Sortable {
    columns: Column[];

    Sort(): void;
}