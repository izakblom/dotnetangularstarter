import { Injectable } from '@angular/core';
import { UserProfile } from '../shared/models/userProfile';

/**
 * Service to unify all localStorage reads and writes accross components.
 * Add a key here as well as getters and setters, so values can be used consistently
 * between components or services
 */
@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  readonly KEYS = {
    SIGNUP_DETAILS: 'SIGNUP_DETAILS',
    LS_JWT: 'LS_JWT',
    USER_PROFILE: 'USER_PROFILE'
  }

  constructor() { }

  /**
   * Method to clear all user-specific local storage objects on signout
   */
  public clearStorageSignout() {
    // console.log('localStorage clearStorageSignout');

    localStorage.removeItem(this.KEYS.LS_JWT);
    localStorage.removeItem(this.KEYS.USER_PROFILE);
    localStorage.removeItem(this.KEYS.SIGNUP_DETAILS);

  }

  public setSignupDetails(firstName: string, lastName: string, email: string) {
    localStorage.setItem(this.KEYS.SIGNUP_DETAILS, JSON.stringify({ FirstName: firstName, LastName: lastName, Email: email }));
  }

  public getSignupDetails(): { FirstName: string, LastName: string, Email: string } {
    return JSON.parse(localStorage.getItem(this.KEYS.SIGNUP_DETAILS));
  }

  public setUserProfile(userProfile: UserProfile) {
    localStorage.setItem(this.KEYS.USER_PROFILE, JSON.stringify(userProfile));
  }

  public getUserProfile(): UserProfile {
    return JSON.parse(localStorage.getItem(this.KEYS.USER_PROFILE));
  }

  public setJWT(token: string) {
    if (!token || token === '') {
      localStorage.removeItem(this.KEYS.LS_JWT);
    } else {
      localStorage.setItem(this.KEYS.LS_JWT, token);
    }
  }

  public getJWT(): string {
    return localStorage.getItem(this.KEYS.LS_JWT);
  }
}
