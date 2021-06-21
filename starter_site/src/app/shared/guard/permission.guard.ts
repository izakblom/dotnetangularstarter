import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { NotifyService } from 'src/app/services/notifyService';
import { Permission } from '../models/permission';
import { PermissionsService } from 'src/app/services/permissions.service';
import { Observable } from 'rxjs';
import { FeatureGuard } from '../models/featureGuard';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {

  permissions: Permission[];
  featureGuards: FeatureGuard[];

  constructor(
    private userService: UserService,
    public router: Router,
    private notifyService: NotifyService,
    private permissionsService: PermissionsService
  ) {
    this.permissionsService.getAllPermissions().then(permissions => {
      this.permissions = permissions;
    });
    this.permissionsService.getAllFeatureGuards().then(guards => {
      this.featureGuards = guards;
    });
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return new Promise<boolean>((resolve1, reject) => {
      const user = this.userService.getProfileCache();
      if (user) {
        console.log('state.url: ', state.url);
        this.permissionsService.getAllFeatureGuards().then(guards => {
          this.featureGuards = guards;
          this.verifyRoutePermissions(state.url).then(valid => {
            // console.log('verify route permissions state: ', valid);
            if (!valid) {
              this.notifyService.showErrorNotification('You are not authorized to access the requested resource', 6000);
            }
            resolve1(valid);
          });
        });
      } else {
        this.notifyService.showErrorNotification('You are not authorized to access the requested resource', 6000);
        this.router.navigate(['profile']);
        resolve1(false);
      }
    });

  }

  private async verifyRoutePermissions(activeRoute: string): Promise<boolean> {
    // return new Promise((resolve2, reject) => {
    // console.log('verifyRoutePermissions');


    const applicableFeatureGuards: FeatureGuard[] = [];
    for (const featGuard of this.featureGuards) {
      // console.log('iterating featGuard: ', featGuard);
      if (featGuard.urlRegex) {
        const regexMatches = activeRoute.match(featGuard.urlRegex);
        // console.log('regexMatches: ', regexMatches);

        if (regexMatches && regexMatches.length > 0) {
          applicableFeatureGuards.push(featGuard);
        }
      }

    }
    console.log('applicableFeatureGuards: ', applicableFeatureGuards);

    if (applicableFeatureGuards && applicableFeatureGuards.length > 0) {
      // console.log('mapping: ', mapping);
      // console.log('user permissions: ', permissions);
      let requirementsMet = true;
      for (const featureGuard of applicableFeatureGuards) {
        if (featureGuard.permissionsAll && featureGuard.permissionsAll.length > 0) {
          const hasPermisionsForGuard = await this.userService.userHasPermissions(
            featureGuard.permissionsAll);

          if (!hasPermisionsForGuard) {
            requirementsMet = false;
          }
        } else if (featureGuard.permissionsAny && featureGuard.permissionsAny.length > 0) {
          let atLeastOne = false;
          for (const perm of featureGuard.permissionsAny) {
            const hasPermisionsForGuard = await this.userService.userHasPermissions(
              [perm]);
            if (hasPermisionsForGuard) {
              atLeastOne = true;
            }
          }
          requirementsMet = atLeastOne;
        }

      }
      // console.log('guard result: ', requirementsMet);

      return requirementsMet;

    } else {
      return false;
    }
    // });


  }
}
