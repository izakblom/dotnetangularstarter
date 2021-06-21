import { Component, OnInit, ViewChild, Input, AfterViewInit, EventEmitter, Output } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TableColumn } from '../models/tableColumnDefinition';
import { Subscription, merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { DataTableService } from '../dataTable.service';
import { DatatableDataSource } from '../models/datatableDataSource';
import { ApiService } from 'src/app/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';

export interface DataTableCustomAction {
  actionHeading: string;
  actionCaption: string;
  actionCallback: (data: any) => void;
}

@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.css']
})
export class DataTableComponent implements OnInit, AfterViewInit {

  @Input() apiDataRoute: string;
  @Input() enableClickEvents = false;
  @Input() title = '';
  // Specify filter key for the table so local storage filter params can be loaded correctly if multiple tables on one component/route
  @Input() filterKey = '';
  @Input() filtersEnabled = true;
  @Input() enableSelection = false;
  @Input() refreshEnabled = false;
  // Specify selection action button text
  @Input() selectionActionTxt = '';
  @Input() smallText = false;

  @Input() enableCustomActions = false;
  @Input() customActions: DataTableCustomAction[] = [];
  @Output() rowClick = new EventEmitter<any>();
  // Selection made and accepted
  @Output() selectionAccepted = new EventEmitter<any>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  fontSizeInPx = 14;
  columnDefs: string[];
  columns: TableColumn[];
  dataSource: any;
  totalCount: number;
  showLoading = false;

  columnsSubscription: Subscription;

  filter: any = {};

  filterChangeTimeout;

  showFilters = false;

  selection = new SelectionModel<any>(true, []);



  constructor(
    private dataTableService: DataTableService,
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    if (this.filterKey === '') {
      this.filterKey = this.title;
    }
    if (this.smallText) {
      this.fontSizeInPx = 10;
    }
    // First load the filter from query params or local storage, before setting up the table
    this.setupFilterFromQueryParams().then(() => {
      this.setupDataSource();
      this.dataSource.loadResults(this.filter);
    });
  }

  /**
   * Creates the table datasource by providing the required dependency to the datatable service
   * and the input parameter apiDataRoute.
   * Sets up the required subscriptions to the Subjects defined in the datasource and assigns
   * the paginator and sort targets
   */
  private setupDataSource() {
    this.dataSource = new DatatableDataSource(this.dataTableService, this.apiDataRoute);
    this.columnsSubscription = this.dataSource.columns.subscribe(result => {
      // console.log('got columns: ', result);
      if (result.length > 0) {
        this.columns = result;
        this.columnDefs = this.enableSelection ? ['select'] : [];
        for (const col of this.columns) {
          if (col.visible) {
            this.columnDefs.push(col.propertyName);
            if (col.searchable && col.searchOptionsURL) {
              // Multi-select, load the filter options from url
              this.loadLookupFilterOptions(col);
            }
          }
        }
        if (this.enableCustomActions) {
          for (let i = 0; i < this.customActions.length; ++i) {
            this.columnDefs.push('custom' + i);
          }

        }

        // this.columnDefs = result.map(dc => dc.visible ? dc.propertyName : null);
        this.columnsSubscription.unsubscribe();
      }
    });
    this.dataSource.loading.subscribe(loading => { this.showLoading = loading; });
    this.dataSource.totalCount.subscribe(result => { this.totalCount = result; });

    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Loads the filter object from queryParams by checking if any properties exist on the queryParams
   * object and setting those properties on the filter object if they exist. This prevents ngModel bindings
   * from breaking if the filter object is simply reassigned in its entirety.
   *
   * The filter object is also saved to local storage if any properties existed on the queryParams object.
   * If no queryParams properties are found, the filter object is loaded from local storage, if an entry is found and then
   * appended to the url for consistency.
   *
   * NB This method must only be called once on startup to prevent infinite recursion
   */
  private setupFilterFromQueryParams() {
    const that = this;
    let queryParamsPropertyCount = 0;
    return new Promise((resolve, reject) => {
      if (this.filtersEnabled) {
        this.route.queryParams.subscribe(params => {
          for (const property in params) {
            if (params.hasOwnProperty(property)) {
              queryParamsPropertyCount++;
              that.filter[property] = params[property];
            }
          }
          // console.log('filter: ', that.filter);
          if (queryParamsPropertyCount === 0) {
            // Check in local storage
            const lsFilter = JSON.parse(
              localStorage.getItem(this.router.routerState.snapshot.url.toString().split('?')[0] + '_filter' + that.filterKey));
            if (lsFilter) {
              that.filter = lsFilter;
            }
          } else {
            // Save to local storage
            localStorage.setItem(
              that.router.routerState.snapshot.url.toString().split('?')[0] + '_filter' + that.filterKey, JSON.stringify(that.filter));
          }

        }).unsubscribe();
        if (queryParamsPropertyCount === 0) {
          // loaded from local storage, so append to url for consistency
          that.router.navigate(['./'], { queryParams: that.filter, relativeTo: that.route });
        }
      }

      resolve();
    });
  }

  ngAfterViewInit() {
    // reset the paginator after sorting
    this.sort.sortChange.subscribe((sort) => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.loadResultsPage())
      )
      .subscribe();
  }

  loadResultsPage() {
    this.selection.clear();
    this.dataSource.loadResults(
      this.filter, this.sort.active,
      this.sort.direction,
      this.paginator.pageIndex,
      this.paginator.pageSize);
  }


  onRowClick(row) {
    if (this.enableClickEvents) {
      this.rowClick.emit(row);
    }
  }

  handleCustomAction(event, customAction: DataTableCustomAction, element: any) {
    event.stopPropagation();
    console.log('custom action element: ', element);
    customAction.actionCallback(element);
  }

  /**
   * Triggered on any filter input change. Uses timeout function to rate limit
   * input events to prevent excessive api calls. Only after 600 milliseconds of no typing will
   * api call fire
   */
  onFiltersChange(event) {
    // console.log('filter: ', this.filter);
    if (this.filterChangeTimeout) {
      // Cancel previous timeout
      clearTimeout(this.filterChangeTimeout);
    }
    // Create 600 milliseconds timeout before refreshing to account for more typing
    this.filterChangeTimeout = setTimeout(() => {
      this.filterChangeTimeout = null;
      this.appendFilterToQueryParams();
      this.loadResultsPage();
    }, 600);

  }

  /**
   * Appends the current filter object to queryParams in the url, WITHOUT RELOADING THE COMPONENT
   * Also saves the filter object in local storage
   */
  private appendFilterToQueryParams() {
    // Save filter object to local storage
    // Use route without queryparams as key, since this component can be used all over and incorret keys will be used
    localStorage.setItem(
      this.router.routerState.snapshot.url.toString().split('?')[0] + '_filter' + this.filterKey, JSON.stringify(this.filter));
    // Navigate to same route will only append queryParams, component does not reload
    this.router.navigate(['./'], { queryParams: this.filter, relativeTo: this.route });
  }

  clearFilter() {
    this.columns.forEach(col => {
      if (col.searchable) {
        this.filter[col.propertyName] = '';
      }
    });
    this.appendFilterToQueryParams();
    this.loadResultsPage();
  }

  /**
   * Loads the filter options for a multi-option lookup filter
   * @param column Contains the filter specification
   */
  loadLookupFilterOptions(column: TableColumn) {
    // console.log('load filter options for ', column);

    this.apiService.get(column.searchOptionsURL).subscribe(result => {
      this.columns[this.columns.indexOf(column)].searchOptions = result;
    }, error => {
      console.error('error in loadLookupFilterOptions: ', error);

    });
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  /**
   * Determines if any filters are active by looping through each property (if any) of the
   * filter object and determining if the value of the property is not null or ''
   */
  areFiltersActive() {
    let filterCount = 0;
    for (const property in this.filter) {
      if (this.filter.hasOwnProperty(property)) {
        if (this.filter[property] != null && this.filter[property] !== '') {
          if (this.columns && this.columns.length > 0) {
            if (this.columns.find(c => c.searchable && c.propertyName === property) !== undefined) {
              filterCount++;
            }
          } else {
            filterCount++;
          }
        }
      }
    }
    return filterCount > 0;
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  acceptSelection() {
    this.selectionAccepted.emit(this.selection.selected);
  }

  onRefresh() {
    this.loadResultsPage();
  }
}
