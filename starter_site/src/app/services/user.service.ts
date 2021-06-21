import { Injectable, EventEmitter } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserProfile } from '../shared/models/userProfile';
import { DynamicFormElement } from '../dynamic-forms/shared/dynamicFormElement';
import { LocalStorageService } from './localStorage.service';
import { NavItem } from '../shared/models/navItem';
import { Permission } from '../shared/models/permission';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly API_BASE = 'api/users';
  private user: UserProfile;

  permissionsChanged = new EventEmitter<boolean>();

  constructor(private apiService: ApiService, private localStorageService: LocalStorageService) { }

  getProfile(): Observable<UserProfile> {
    return this.apiService.get(this.API_BASE + '/GetUser', null, null, true).pipe(
      map(user => {
        // console.log('userService getProfile user: ', user);

        this.localStorageService.setUserProfile(user);
        this.user = user;
        return user;
      })
    );
  }

  profileRetrieved() {
    return this.user !== null;
  }

  getPermissionedMenu(): Observable<NavItem[]> {
    return this.apiService.get('api/home' + '/GetPermissionedMenu');
  }

  getProfileCache(): UserProfile {
    if (!this.user) {
      return this.localStorageService.getUserProfile();
    }
    return this.user;
  }

  getProfileForm(): Observable<{ elements: DynamicFormElement<any>[] }> {
    return this.apiService.get(this.API_BASE + '/GetUserProfileForm', null, null, true);
  }

  createUpdateProfile(userData: UserProfile) {
    return this.apiService.post(this.API_BASE + '/CreateUpdateUser', userData, null, true);
  }

  userHasPermission(permission: Permission) {
    return new Promise<boolean>((resolve, reject) => {
      if (!this.user) {
        this.getProfile().subscribe((result) => {
          if (this.user.permissions.find(perm => perm.id === permission.id) !== undefined) {
            resolve(true);
          }
          resolve(false);
        });
      } else {
        resolve(this.user.permissions.find(perm => perm.id === permission.id) !== undefined);
      }
    });

  }

  userHasPermissions(permissions: Permission[]): Promise<boolean> {
    // console.log('userHasPermissions checking permissions: ', permissions);
    // console.log('this.user.permissions', this.user.permissions);


    return new Promise((resolvePerms, reject) => {
      let result = true;
      if (!this.user) {
        this.getProfile().subscribe((profileResult) => {
          for (const permission of permissions) {
            if (this.user.permissions.find(perm => perm.id === permission.id) === undefined) {
              result = false;
            }
          }
          resolvePerms(result);
        });
      } else {
        for (const permission of permissions) {
          if (this.user.permissions.find(perm => perm.id === permission.id) === undefined) {
            // console.log('resolve false');

            result = false;
          }
        }
        // console.log('resolve result: ', result);
        resolvePerms(result);
      }
    });

  }

  /**
   * Update the user's permissions with the provided
   * @param permissions The permissions to be assigned
   */
  updateUserPermissions(permissions: Permission[]) {
    if (this.user) {
      this.user.permissions = permissions.slice();
      this.localStorageService.setUserProfile(this.user);
      // Notify app component to reload permissioned menu
      this.permissionsChanged.emit(true);
    } else {
      this.user = this.localStorageService.getUserProfile();
      if (this.user) {
        this.user.permissions = permissions.slice();
        this.localStorageService.setUserProfile(this.user);
        // Notify app component to reload permissioned menu
        this.permissionsChanged.emit(true);
      }
    }

  }

}
