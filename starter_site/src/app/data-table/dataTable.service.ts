import { Injectable } from '@angular/core';
import { ApiService } from '../services/api.service';
import { DataTableFilter } from './models/dataTableFilter';
import { Observable } from 'rxjs';
import { TableDefinition } from './models/tableDefinition';

@Injectable()
export class DataTableService {

  constructor(private apiService: ApiService) {

  }

  loadDataTableResults(route: string, filter: DataTableFilter): Observable<TableDefinition> {
    return this.apiService.post(route, filter);
  }
}
