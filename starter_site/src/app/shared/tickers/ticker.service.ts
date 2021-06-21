import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Observable } from 'rxjs';
import { TickerFilter } from './TickerFilter';
import { DashboardContent } from './DashboardContent';

@Injectable({
  providedIn: 'root'
})
export class TickerService {

  readonly API_BASE = 'api/ticker';

  constructor(private apiService: ApiService) {

  }

  getTickers(): Observable<any> {
    return this.apiService.get(this.API_BASE + '/GetTickers', null, null, true);
  }

  getTickerData(filter: TickerFilter): Observable<any> {
    return this.apiService.post(this.API_BASE + '/GetTickerData', filter, null, false);
  }

  getTickerFilterData(tickerId: string, filterTypeId: string, filter: string): Observable<any> {
    return this.apiService.get(this.API_BASE + '/GetTickerFilterData', [
      { key: 'tickerId', value: tickerId },
      { key: 'filterTypeId', value: filterTypeId },
      { key: 'filter', value: filter },
    ], null, true);
  }

  getUserDashboard(route: string): Observable<any> {
    return this.apiService.get(this.API_BASE + '/GetUserDashboard', [{ key: 'route', value: route }], null, true);
  }

  updateUserDashboard(dash: DashboardContent): Observable<any> {
    // console.log('Saving dashboard')
    // console.log(dash);
    return this.apiService.post(this.API_BASE + '/UpdateUserDashboard', dash, null, true);
  }

}
