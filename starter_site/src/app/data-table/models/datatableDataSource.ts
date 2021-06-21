import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { CollectionViewer } from '@angular/cdk/collections';
import { DataTableService } from '../dataTable.service';
import { DataTableFilter } from './dataTableFilter';
import { TableColumn } from './tableColumnDefinition';

export class DatatableDataSource implements DataSource<any> {

  private resultsSubject = new BehaviorSubject<any[]>([]);
  private columnsSubject = new BehaviorSubject<TableColumn[]>([]);
  private totalCountSubject = new BehaviorSubject<number>(0);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading = this.loadingSubject.asObservable();
  public columns = this.columnsSubject.asObservable();
  public totalCount = this.totalCountSubject.asObservable();
  public data: any;

  constructor(private dataTableService: DataTableService, private apiDataRoute: string) { }

  connect(collectionViewer: CollectionViewer): Observable<any[]> {
    return this.resultsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.resultsSubject.complete();
    this.loadingSubject.complete();
    this.columnsSubject.complete();
  }

  public loadResults(filter = null, sortColumn = '',
    sortDirection = 'asc', pageIndex = 0, pageSize = 10) {
    // console.log('sortColumn: ', sortColumn);

    this.loadingSubject.next(true);

    this.dataTableService.loadDataTableResults(
      this.apiDataRoute,
      new DataTableFilter(pageIndex * pageSize, pageSize, sortColumn, sortDirection, filter))
      .subscribe(result => {
        // console.log('result of loadDataObservable: ', result);

        this.columnsSubject.next(result.columns);
        this.totalCountSubject.next(result.totalRecordCount);
        this.loadingSubject.next(false);
        this.data = result.data;
        this.resultsSubject.next(result.data);
      }, error => {
        this.loadingSubject.next(false);
      });
  }
}
