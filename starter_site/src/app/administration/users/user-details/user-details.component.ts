import { Component, OnInit, NgZone } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/shared/models/user';
import { Role } from 'src/app/shared/models/role';
import { Permission } from 'src/app/shared/models/permission';
import { NotifyService } from 'src/app/services/notifyService';
import { UserService } from 'src/app/services/user.service';
import { UserProfile } from 'src/app/shared/models/userProfile';
import { Location } from '@angular/common';
import { PermissionsService } from 'src/app/services/permissions.service';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  private readonly DISABLE_USER_FEATURE_NAME = 'DISABLE_USER';
  private readonly MANAGE_USER_PERMISSIONS_FEAUTURE_NAME = 'SET_USER_PERMISSIONS';

  userId: string;
  user: User;
  roles: Role[];
  permissions: Permission[];
  permissionsWithoutRoles: Permission[];

  currentUser: UserProfile;

  hasManageUserPermission = false;
  hasManagePermsPermission = false;



  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private notifyService: NotifyService,
    private ngZone: NgZone,
    private userService: UserService,
    private location: Location,
    private permissionsService: PermissionsService,
    private router: Router
  ) { }

  async ngOnInit() {
    this.getCurrentUser();
    this.userId = this.route.snapshot.params.id;
    // Check if the user has permissions to use the feature for disabling a user
    const disableUserFeatGuard = await this.permissionsService.findFeatureMappingByFeatureName(this.DISABLE_USER_FEATURE_NAME);
    if (disableUserFeatGuard) {
      this.hasManageUserPermission = await this.userService.userHasPermissions(disableUserFeatGuard.permissionsAll);
    }
    // Check if the user has permissions to use the feature for managing user permissions
    const manageUserPermissionsGuard = await this.permissionsService.findFeatureMappingByFeatureName(
      this.MANAGE_USER_PERMISSIONS_FEAUTURE_NAME);
    if (manageUserPermissionsGuard) {
      this.hasManagePermsPermission = await this.userService.userHasPermissions(manageUserPermissionsGuard.permissionsAll);
    }
    if (this.hasManagePermsPermission) {
      this.loadAllRolesAndPermissions();
    }
    await this.loadUserDetails();


  }



  private getCurrentUser() {
    this.currentUser = this.userService.getProfileCache();


  }

  private loadAllRolesAndPermissions() {
    return new Promise((resolve, reject) => {
      this.adminService.getAllRolesAndPermissions().subscribe(result => {
        this.roles = result.roles;
        this.permissions = result.permissions;
        this.permissionsWithoutRoles = [];
        this.permissions.forEach(permission => {
          if (this.roles.find(rol => rol.permissions.find(perm => perm.id === permission.id) !== undefined) === undefined) {
            this.permissionsWithoutRoles.push(permission);
          }
        });
        resolve();
      });
    });

  }

  private loadUserDetails() {
    return new Promise((resolve, reject) => {
      this.adminService.getUser(this.userId).subscribe(result => {
        this.user = result;
        // console.log('user: ', this.user);
        resolve();
      });
    });
  }

  isRoleAssigned(role: Role) {
    return this.user && role.permissions.every(perm => {
      return this.user.permissions.find(userperm => {
        return userperm.id === perm.id;
      }) !== undefined;
    });
  }

  onRoleToggle(role: Role) {
    this.adminService.assignRevokeRole(this.userId, role.id).subscribe(permissions => {
      if (permissions) {
        this.user.permissions = permissions;
        // If the current user is the user subject, reload current user's permissions
        if (this.currentUser.id === this.user.id) {
          this.userService.updateUserPermissions(permissions);
          this.location.back();
        }
      }
    }, error => {
      this.resetPermissionTogglesOnError();
      this.notifyService.showErrorNotification(`Updating role "${role.name}" failed.`);
    });
  }

  onPermissionToggle(permission: Permission) {

    this.adminService.assignRevokePermission(this.userId, permission.id).subscribe(permissions => {
      // console.log('assignRevoke result: ', result);
      this.user.permissions = permissions;
      // If the current user is the user subject, reload current user's permissions
      if (this.currentUser.id === this.user.id) {

        this.userService.updateUserPermissions(permissions);
        this.location.back();
      }
    }, error => {
      this.resetPermissionTogglesOnError();
      this.notifyService.showErrorNotification(`Updating permission "${permission.name}" failed.`);
    });
  }

  /**
   * In case of api error, this method resets all toggle-switches to the values they were before one was
   * switched to trigger the api call.
   */
  private resetPermissionTogglesOnError() {
    const that = this;
    const permissions = this.user.permissions.slice();
    this.user.permissions = []; // Force view update
    this.ngZone.run(() => {
      // Use timeout to ensure view gets updated
      setTimeout(() => {
        // Reset to previous value
        that.user.permissions = permissions;
      }, 25);

    });
  }

  userHasPermission(permission: Permission): boolean {
    return this.user.permissions.find(perm => perm.id === permission.id) !== undefined;
  }

  onEnableDisableUser() {
    this.adminService.enableDisableUser(this.user.id).subscribe(result => {
      if (result) {
        const text = this.user.isActive ? 'disabled' : 'enabled';
        this.user.isActive = !this.user.isActive;
        this.notifyService.showNotification('The user profile has been ' + text);
      }
    });
  }

}
