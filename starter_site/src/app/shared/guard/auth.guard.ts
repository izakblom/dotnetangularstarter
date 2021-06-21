import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private userService: UserService, public router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return new Promise((resolve, reject) => {
      if (!this.authService.isLoggedIn()) {
        // console.log('authguard triggering signout');

        this.authService.SignOut(); // clear local storage and delete token
        resolve(false);
      } else {
        // console.log('authGuard profileRetrieved: ', this.userService.profileRetrieved());
        // Check if email is verified
        this.authService.isEmailVerified().then(emailVerified => {
          if (!emailVerified) {
            resolve(false);
          }
          // console.log('authguard emailVerified: ', emailVerified);

          // console.log('authguard resolving: ', emailVerified && this.userService.profileRetrieved());

          resolve(emailVerified && this.userService.profileRetrieved());
        });
      }
    });

  }
}
