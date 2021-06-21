import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { LocalStorageService } from 'src/app/services/localStorage.service';
import { UserProfile } from 'src/app/shared/models/userProfile';
import { AuthService } from 'src/app/services/auth.service';
import { NotifyService } from 'src/app/services/notifyService';
import { DynamicFormElement } from 'src/app/dynamic-forms/shared/dynamicFormElement';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-setup',
  templateUrl: './profile-setup.component.html',
  styleUrls: ['./profile-setup.component.css']
})
export class ProfileSetupComponent implements OnInit {
  user: UserProfile;
  profileDynamicForm: { elements: DynamicFormElement<any>[] };
  jwtUser: { uid: string, email: string, emailVerified: boolean };


  constructor(
    private userService: UserService,
    private localStorageService: LocalStorageService,
    private authService: AuthService,
    private notifyService: NotifyService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getUserProfile();
    // Get the JWT provider data so we can populate email from there as well
    this.jwtUser = this.authService.getJWTUserData();
  }

  /**
   * Load user profile form from server
   */
  private getUserProfile() {
    this.user = this.userService.getProfileCache();
    this.userService.getProfileForm().subscribe(result => {
      // console.log('result: ', result);
      this.profileDynamicForm = result;
      if (!this.user || !this.user.complete) {
        this.populateWithSignupDetails();
      }
    }, error => {
      console.error('error in getUserProfile: ', error);
      this.notifyService.showErrorNotification('Your profile details could not be loaded');
    });
  }

  /**
   * Load signup details as stored in local strorage during signup
   */
  private populateWithSignupDetails() {
    const signupDetails = this.localStorageService.getSignupDetails();
    if (signupDetails) {
      this.profileDynamicForm.elements.find(el => el.key === 'FirstName').value = signupDetails.FirstName;
      this.profileDynamicForm.elements.find(el => el.key === 'LastName').value = signupDetails.LastName;
      this.profileDynamicForm.elements.find(el => el.key === 'Email').value = signupDetails.Email;
    }
  }


  onSubmit(formVal) {
    // Add verification status to dto
    formVal.Validated = this.jwtUser.emailVerified;
    this.userService.createUpdateProfile(formVal).subscribe(result => {
      this.notifyService.showNotification('Profile updated');
      if (!this.user || !this.user.complete) {
        // If the user profile was not previously retrieved, this must be first sign-in profile update
        // IF update successful reload so the menu can be initialised
        // Use timeout so notification is displayed first
        setTimeout(() => {
          location.reload();
        }, 1000);
      } else {
        // Just reload the form from server to ensure data was updated correctly
        this.ngOnInit();
      }
    }, error => {
      console.error('error updating: ', error);

      this.notifyService.showErrorNotification('Profile update failed');
    });
  }

}
