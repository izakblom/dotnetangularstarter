import { FilterType, FilterOptionsOption } from './TickerTemplate';

export class TickerFilter {
    public selectedDateRangeFilter: number;
    public from: string;
    public to: string;
    public textFilter: string;
    public selectedTicker: number;
    public collectionFilters: CollectionFilter[] = [];
    public templateId: number;
}

export class CollectionFilter {
    public type: FilterType;
    public items: FilterOptionsOption[] = [];
}
