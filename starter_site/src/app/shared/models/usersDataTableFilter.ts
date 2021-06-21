import { DataTableFilter } from 'src/app/data-table/models/dataTableFilter';
import { filter } from 'rxjs/operators';

export class UsersDataTableFilter extends DataTableFilter {
  public filter: { firstName: string, lastName: string, email: string };

  /**
   *
   */
  constructor(skip: number, take: number, sortColumnName, sortDirection) {
    super(skip, take, sortColumnName, sortDirection);

  }
}
