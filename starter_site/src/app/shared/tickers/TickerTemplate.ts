export class TickerTemplate {

    public category: TickerTemplateCategory;
    public type: TickerTemplateType;
    public currency: boolean;
    public filterByDateRange: boolean;
    public id: number;
    public sensitive: boolean;
    public title: string;
    public permissions: TickerPermission[];
    public availableFilters: AvailableFilter[];
}

export class TickerTemplateCategory {
    public id: number;
    public name: string;
}

export class TickerTemplateType {
    public id: number;
    public name: string;
}

export class TickerPermission {
    public id: number;
    public name: string;
}

export class AvailableFilter {
    public description: string;
    public type: FilterType;
    public options: FilterOptions;
}

export class FilterType {
    public id: number;
    public name: string;
}

export class FilterOptions {
    public lookup: boolean;
    public remoteFetch: boolean;
    public options: FilterOptionsOption[];

}

export class FilterOptionsOption {
    public id: number;
    public value: string;
}

