import { TableColumn } from './tableColumnDefinition';

export class TableDefinition {
  public data: any[];
  public columns: TableColumn[];
  public filteredRecordCount: number;
  public totalRecordCount: number;
}

