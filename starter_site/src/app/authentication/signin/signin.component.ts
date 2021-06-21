import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { LocalStorageService } from 'src/app/services/localStorage.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {
  form: FormGroup;
  authError = null;
  signinPending = false;
  showForgotPW = false;

  resetPWEmail = '';
  resetPending = false;
  resetInvalidEmailErr = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private lsService: LocalStorageService
  ) { }

  ngOnInit() {
    // console.log('ngOnInit signin');

    const signupDetails = this.lsService.getSignupDetails();
    this.form = new FormGroup({
      email: new FormControl(signupDetails ? signupDetails.Email : '', [
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$')
      ]),
      password: new FormControl('', [
        Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9#?!@$%^&*+,-./:;<=>\[\\\]_`{|}~]+$')
      ]),
    });
  }

  async submit() {
    try {
      this.signinPending = true;
      const emailVerified = await this.authService.SignIn(this.form.value.email, this.form.value.password);
      this.signinPending = false;
      if (emailVerified) {
        this.router.navigate(['/profile']);
      }
    } catch (error) {
      this.signinPending = false;
      console.error('error in sigin: ', error);
      if (error.code && error.code === 'auth/user-disabled') {
        this.authError = 'Your account has been disabled, please contact support';
      } else if (error.code === 'auth/wrong-password' || error.code === 'auth/user-not-found') {
        this.authError = 'Incorrect credentials';
      } else {
        this.authError = error.message;
      }

    }

  }

  onSignup() {
    this.router.navigate(['register']);
  }

  onForgotPassword() {
    // this.router.navigate(['forgot-password']);
    this.showForgotPW = true;
  }

  onResetPW() {
    this.resetInvalidEmailErr = false;
    this.authService.ForgotPassword(this.resetPWEmail).then(() => {
      this.resetPending = true;
    }).catch(error => {
      this.resetInvalidEmailErr = true;
    });

  }

}
