import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-verifyemail',
  templateUrl: './verifyemail.component.html',
  styleUrls: ['./verifyemail.component.css']
})
export class VerifyemailComponent {

  sendVerifyPending = false;
  verifySent = false;

  constructor(private router: Router, private authService: AuthService) { }


  onContinue() {
    this.router.navigate(['signin']);
  }

  onResend() {
    this.sendVerifyPending = true;
    this.authService.SendVerificationMail().then(() => {
      this.sendVerifyPending = false;
      this.verifySent = true;
    }).catch(error => {
      this.sendVerifyPending = false;
    });
  }

}
