import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SignupComponent } from './signup/signup.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SigninComponent } from './signin/signin.component';
import { MaterialModule } from '../material/material.module';
import { VerifyemailComponent } from './verifyemail/verifyemail.component';
import { ClarityModule } from '@clr/angular';

@NgModule({
  declarations: [SignupComponent, SigninComponent, VerifyemailComponent],
  imports: [
    CommonModule,
    RouterModule,
    MatFormFieldModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    ClarityModule
  ]
})
export class AuthenticationModule { }
