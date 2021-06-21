import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SigninComponent } from './authentication/signin/signin.component';
import { SignupComponent } from './authentication/signup/signup.component';
import { AuthGuard } from './shared/guard/auth.guard';
import { HomeComponent } from './home/home.component';
import { VerifyemailComponent } from './authentication/verifyemail/verifyemail.component';
import { ProfileSetupComponent } from './profile/profile-setup/profile-setup.component';
import { PermissionGuard } from './shared/guard/permission.guard';
import { TermsandconditionsComponent } from './client-pages/termsandconditions/termsandconditions.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'signin', component: SigninComponent },
  { path: 'register', component: SignupComponent },
  { path: 'verify-email', component: VerifyemailComponent },
  { path: 'profile', component: ProfileSetupComponent, canActivate: [AuthGuard] },
  { path: 'admin', loadChildren: './administration/administration.module#AdministrationModule', canActivate: [AuthGuard, PermissionGuard] },
  { path: 'termsandconditions', component: TermsandconditionsComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
