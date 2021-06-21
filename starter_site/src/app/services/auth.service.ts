import { Injectable, NgZone, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { AngularFireAuth } from '@angular/fire/auth';
import { LocalStorageService } from './localStorage.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  userData: any;
  token: string;
  profileRetrieved = false;

  public AuthChange = new EventEmitter<{ authenticated: boolean, email: string }>();

  constructor(
    public fireAuth: AngularFireAuth,
    public router: Router,
    private ngZone: NgZone,
    private locStorageService: LocalStorageService
  ) {

    this.fireAuth.authState.subscribe(user => {
      this.ngZone.run(() => {
        if (user) {
          this.userData = user;
        }
      });
    });
    this.loadFirebaseUser();
  }

  private loadFirebaseUser() {
    return new Promise((resolve, reject) => {
      if (!this.userData) {
        this.fireAuth.user.subscribe(user => {
          this.userData = user;
          resolve();
        });
      } else {
        resolve();
      }
    });
  }

  public getJWTUserData(): { uid: string, email: string, emailVerified: boolean } {
    return this.userData;
  }

  async SignIn(email, password) {
    try {
      const result = await this.fireAuth.auth.signInWithEmailAndPassword(email, password);
      // console.log('Signin result: ', result);
      this.storeToken();
      if (!this.userData.emailVerified) {
        this.router.navigate(['verify-email']);
        return false;
      }

      this.AuthChange.emit({ authenticated: true, email: email });
      return true;
    } catch (error) {
      console.error('error in Signin: ', error);
      throw error;
    }
  }

  /**
   * Refresh the token, store in local storage and return the token
   */
  refreshToken(): Promise<string> {
    if (environment.stage === 'dev') {
      // console.log('refreshing token...');
    }
    // console.log('refreshing token');

    return new Promise((resolve, reject) => {
      if (this.fireAuth.auth.currentUser) {
        this.fireAuth.auth.currentUser.getIdToken(true).then(token => {
          // console.log('got token');
          this.token = token;
          this.locStorageService.setJWT(token);
          resolve(token);
        }).catch(error => {
          reject(error);
          console.error('error in refreshToken: ', error);
        });
      } else {
        reject();
      }

    });

  }

  storeToken() {
    this.fireAuth.auth.currentUser.getIdToken().then(token => {
      this.ngZone.run(() => {
        this.token = token;
        this.locStorageService.setJWT(token);
      });
    }).catch(error => {
      console.error('error in storeToken: ', error);
    });
  }

  async SignUp(firstName, lastName, email, password) {
    try {
      const result = await this.fireAuth.auth.createUserWithEmailAndPassword(email, password);
      this.locStorageService.setSignupDetails(firstName, lastName, email);
      this.SendVerificationMail();
    } catch (error) {
      console.error('error in Signup: ', error);
      throw error;
    }
  }


  SendVerificationMail() {
    return this.fireAuth.auth.currentUser.sendEmailVerification();

  }


  ForgotPassword(passwordResetEmail) {
    return this.fireAuth.auth.sendPasswordResetEmail(passwordResetEmail);
  }


  isLoggedIn(): boolean {
    // console.log('isLoggedIn');

    const jwt = this.locStorageService.getJWT();

    return jwt !== null;
  }

  async isEmailVerified() {
    await this.loadFirebaseUser();
    return this.userData && this.userData.emailVerified;
  }

  SignOut() {
    // console.log('auth.service Signout');

    return this.fireAuth.auth.signOut().then(() => {
      this.ngZone.run(() => {
        this.locStorageService.clearStorageSignout();
        this.AuthChange.emit({ authenticated: false, email: '' });
        this.router.navigate(['signin']);
      });

    });
  }
}
