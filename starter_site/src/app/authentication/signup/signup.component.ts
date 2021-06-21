import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  form: FormGroup;

  signupErr = null;
  signupPending = false;

  signupDetails = {
    firstName: '',
    lastName: '',
    email: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,

  ) {

  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params && params.u && params.u.length > 0) {
        // console.log(atob(params.u));

        this.signupDetails = JSON.parse(atob(params.u));
        // console.log(this.signupDetails);

      }
      this.buildForm();
    });
  }

  private buildForm() {
    this.form = new FormGroup({
      firstName: new FormControl(this.signupDetails.firstName, [Validators.required]),
      lastName: new FormControl(this.signupDetails.lastName, [Validators.required]),
      email: new FormControl(this.signupDetails.email, [
        Validators.required, Validators.pattern('^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$')
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9#?!@$%^&*+,-./:;<=>\[\\\]_`{|}~]+$'),
        Validators.minLength(8)
      ]),
    });
  }

  async submit() {
    try {
      this.signupPending = true;
      await this.authService.SignUp(this.form.value.firstName, this.form.value.lastName, this.form.value.email, this.form.value.password);
      this.signupPending = false;
      this.router.navigate(['verify-email']);
    } catch (error) {
      this.signupPending = false;
      console.error('error in signup: ', error);
      this.signupErr = error;
    }

  }


}
