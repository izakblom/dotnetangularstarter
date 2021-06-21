import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AvailableFilter, FilterOptionsOption } from '../TickerTemplate';
import { TickerService } from '../ticker.service';
import { FormControl } from '@angular/forms';
import { debounceTime, map, distinctUntilChanged, filter } from 'rxjs/operators';
import { CollectionFilter, TickerFilter } from '../TickerFilter';

@Component({
  selector: 'app-ticker-filter',
  templateUrl: './ticker-filter.component.html',
  styleUrls: ['./ticker-filter.component.css']
})
export class TickerFilterComponent implements OnInit {

  readonly FILTER_TYPES = {
    TEXT_FILTER: 'TEXT_FILTER',
    DATE_FILTER: 'DATE_FILTER',
    COLLECTION_FILTER: 'COLLECTION_FILTER'
  };

  @Input() tickerId: string;
  @Input() filter: AvailableFilter;
  @Input() data: TickerFilter;

  public filterText: FormControl = new FormControl();
  public options: FilterOptionsOption[];

  public availableCollectionFilters: FilterOptionsOption[] = [];
  @Input() selectedCollectionFilters: FilterOptionsOption[] = [];

  @Output() collectionFilterChanged = new EventEmitter();
  @Output() textFilterChanged = new EventEmitter();
  @Output() dateFilterChanged = new EventEmitter();

  public filterType = null;

  public selectedDateFilter: FilterOptionsOption = null;

  constructor(private tickerService: TickerService) { }

  ngOnInit() {
    if (this.filter.type.id === 1 || this.filter.type.id === 2 || this.filter.type.id === 3) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 4) {
      this.filterType = this.FILTER_TYPES.DATE_FILTER;
    } else if (this.filter.type.id === 5) {
      this.filterType = this.FILTER_TYPES.TEXT_FILTER;
    } else if (this.filter.type.id === 6) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 7) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 8) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 9) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 10) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 11) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    } else if (this.filter.type.id === 12) {
      this.filterType = this.FILTER_TYPES.COLLECTION_FILTER;
    }


    this.filterText
      .valueChanges
      .pipe(debounceTime(400))
      .pipe(distinctUntilChanged())
      .subscribe(val => {
        this.getData(val);
      });

    if (this.filterType === this.FILTER_TYPES.COLLECTION_FILTER) {
      for (const c of this.data.collectionFilters) {
        if (c.type.id === this.filter.type.id) {
          this.selectedCollectionFilters = c.items;
        }
      }

      this.filterText.setValue('');
    } else if (this.filterType === this.FILTER_TYPES.DATE_FILTER) {
      if (this.data.selectedDateRangeFilter != null) {
        for (const f of this.filter.options.options) {
          if (this.data.selectedDateRangeFilter === f.id) {
            this.selectedDateFilter = f;
          }
        }
      }
    } else if (this.filterType === this.FILTER_TYPES.TEXT_FILTER) {
      this.filterText.setValue(this.data.textFilter);
    }

  }

  getData(f: string) {
    if (this.filterType === this.FILTER_TYPES.COLLECTION_FILTER) {
      if (this.filter.options.lookup && this.filter.options.remoteFetch) {
        this.tickerService.getTickerFilterData(this.tickerId, this.filter.type.id.toString(), f)
          .subscribe(
            (resp: FilterOptionsOption[]) => {
              this.availableCollectionFilters = resp;
            }
          );
      }
    } else if (this.filterType === this.FILTER_TYPES.TEXT_FILTER) {
      this.textFilterChanged.emit({ filterText: f });
    } else if (this.filterType === this.FILTER_TYPES.DATE_FILTER) {
      // todo nothing, this is not available for dates
    }

  }

  addFilter(selectedF: FilterOptionsOption) {

    if (this.selectedCollectionFilters.find(fi => fi.id === selectedF.id) === undefined) {
      this.selectedCollectionFilters.push(selectedF);
      const e = new CollectionFilter();
      e.type = this.filter.type;
      e.items = this.selectedCollectionFilters;
      this.collectionFilterChanged.emit(e);
    } else {
      // console.log('filter already added!');
    }
  }

  removeFilter(f: FilterOptionsOption) {

    const index = this.selectedCollectionFilters.indexOf(f);

    if (index !== -1) {
      this.selectedCollectionFilters.splice(index, 1);
      const e = new CollectionFilter();
      e.type = this.filter.type;
      e.items = this.selectedCollectionFilters;
      this.collectionFilterChanged.emit(e);
    }
  }

  clearDateFilter() {
    this.data.from = null;
    this.data.to = null;
    this.selectedDateFilter = null;
    this.dateFilterChanged.emit({ dateFilter: this.selectedDateFilter, from: this.data.from, to: this.data.to });
  }

  setDateFilter(f: FilterOptionsOption) {
    this.selectedDateFilter = f;

    if (f.id === 8) {
      this.dateFilterChanged.emit({ dateFilter: this.selectedDateFilter, from: this.data.from, to: this.data.to });
    } else {
      this.data.from = null;
      this.data.to = null;
      this.dateFilterChanged.emit({ dateFilter: this.selectedDateFilter, from: this.data.from, to: this.data.to });
    }
  }

  fromDateChanged(d: any) {
    this.data.from = d;
    this.dateFilterChanged.emit({ dateFilter: this.selectedDateFilter, from: this.data.from, to: this.data.to });
  }

  toDateChanged(d: any) {
    this.data.to = d;
    this.dateFilterChanged.emit({ dateFilter: this.selectedDateFilter, from: this.data.from, to: this.data.to });
  }
}
