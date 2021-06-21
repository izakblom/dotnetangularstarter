import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { LocalStorageService } from './localStorage.service';
import { AuthService } from './auth.service';
import { NotifyService } from './notifyService';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  requestPending = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private locStoreService: LocalStorageService,
    private authService: AuthService,
    private notifyService: NotifyService
  ) {

  }

  doHealthCheck() {
    return this.http.get<any>(environment.api + '/api/values/getprotected', this.options());
  }

  /**
   * Performs a GET request to the provided enpoint. Maps the params array to HTTP query params
   * @param endpoint The API endpoint (no '/' prefix eg. api/values)
   * @param params Array to be mapped to query params
   * @param reqOpts Specify if non-default http options required
   * @param emitProgress Set to true to emit requestPending event
   * emitter fire event to show global progress UI. Only set for requests which should block UI interaction until completed
   */
  get(endpoint: string, params?: { key: string, value: string }[], reqOpts?: any, emitProgress?: boolean): Observable<any> {
    // Append the base api route to endpoint if not present
    if (!endpoint.includes(environment.api)) {
      endpoint = environment.api + '/' + endpoint;
    }
    if (!reqOpts) {
      reqOpts = this.options();
    }
    // Support easy query params for GET requests
    if (params) {
      reqOpts.params = new HttpParams();
      params.forEach(element => {
        reqOpts.params = reqOpts.params.set(element.key, element.value);
      });
    }
    return new Observable((observer) => {
      this.emitPending(true, emitProgress);
      this.http.get<any>(endpoint, reqOpts).subscribe(result => {
        this.emitPending(false, emitProgress);
        observer.next(result);
      }, error => {
        // console.log('error in apiService: ', error);

        if (error.status === 401) {
          // unauthorized, retry
          this.authService.refreshToken().then(token => {
            this.http.get<any>(endpoint, this.options()).subscribe(result => {
              this.emitPending(false, emitProgress);
              observer.next(result);
            }, error2 => {
              this.emitPending(false, emitProgress);
              observer.error(error2);
            });
          }).catch(refreshErr => {
            // console.log('token refresh error: ', refreshErr);
            this.emitPending(false, emitProgress);
            observer.error(error);
            this.authService.SignOut();
          });
        } else {
          if (error.status === 403) { // not authorized, lacks permissions
            this.notifyService.showUnauthorizedNotification();
          }
          this.emitPending(false, emitProgress);
          observer.error(error);
        }
      });
    });
  }

  /**
   * Performs a POST request to the provided enpoint.
   * @param endpoint The API endpoint (no '/' prefix eg. api/values)
   * @param body Request body
   * @param reqOpts Specify if non-default http options required
   * @param emitProgress Set to true to emit requestPending event
   * emitter fire event to show global progress UI. Only set for requests which should block UI interaction until completed
   */
  post(endpoint: string, body: any, reqOpts?: any, emitProgress?: boolean): Observable<any> {
    // Append the base api route to endpoint if not present
    if (!endpoint.includes(environment.api)) {
      endpoint = environment.api + '/' + endpoint;
    }
    if (!reqOpts) {
      reqOpts = this.options();
    }
    return new Observable((observer) => {
      this.emitPending(true, emitProgress);
      this.http.post<any>(endpoint, body, reqOpts).subscribe(result => {
        this.emitPending(false, emitProgress);
        observer.next(result);
      }, error => {
        if (error.status === 401) {
          // unauthorized, retry
          this.authService.refreshToken().then(token => {
            this.http.post<any>(endpoint, body, this.options()).subscribe(result => {

              this.emitPending(false, emitProgress);
              observer.next(result);
            }, error2 => {
              this.emitPending(false, emitProgress);
              observer.error(error2);
            });
          }).catch(refreshErr => {
            // console.log('token refresh error: ', refreshErr);
            this.emitPending(false, emitProgress);
            observer.error(error);
            this.authService.SignOut();
          });
        } else {
          if (error.status === 403) { // not authorized, lacks permissions
            this.notifyService.showUnauthorizedNotification();
          }
          this.emitPending(false, emitProgress);
          observer.error(error);
        }
      });
    });
  }

  private emitPending(pending: boolean, eventEmissionActive: boolean) {
    if (eventEmissionActive) {
      this.requestPending.next(pending);
    }
  }

  options() {
    return {
      headers: this.getAuthHeader()
    };
  }
  optionsForFileUpload() {
    return {
      headers: this.getAuthHeaderForFileUpload()
    };
  }

  public getAuthHeader(): HttpHeaders {

    return new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': 'Bearer ' + this.locStoreService.getJWT()
    });

  }

  public getAuthHeaderForFileUpload(): HttpHeaders {

    const jwt = this.locStoreService.getJWT();
    return new HttpHeaders({
      'Authorization': 'Bearer ' + jwt
    });
  }

  public blobOptions(): any {
    return {
      responseType: 'blob',
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8',
        'Authorization': 'Bearer ' + this.locStoreService.getJWT()
      })
    };
  }

}
