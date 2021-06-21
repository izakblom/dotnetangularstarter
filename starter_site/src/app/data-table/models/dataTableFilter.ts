export class DataTableFilter {
  public filter: any;
  public skip: number;
  public take: number;
  public sortColumnName: string;
  public sortDirection: string;
  /**
   *
   */
  constructor(skip: number, take: number, sortColumnName: string, sortDirection: string, filter?: any) {
    this.skip = skip;
    this.take = take;
    this.sortColumnName = sortColumnName;
    this.sortDirection = sortDirection;
    this.filter = filter;
  }
}
