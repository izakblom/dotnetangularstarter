import { Component, OnInit } from '@angular/core';
import { Role } from 'src/app/shared/models/role';
import { Permission } from 'src/app/shared/models/permission';
import { AdminService } from 'src/app/services/admin.service';
import { ActivatedRoute } from '@angular/router';
import { NotifyService } from 'src/app/services/notifyService';
import { Location } from '@angular/common';

@Component({
  selector: 'app-roles-edit',
  templateUrl: './roles-edit.component.html',
  styleUrls: ['./roles-edit.component.css']
})
export class RolesEditComponent implements OnInit {

  roles: Role[];
  permissions: Permission[];

  role: Role;
  roleId: number;

  constructor(
    private adminService: AdminService,
    private activatedRoute: ActivatedRoute,
    private notifyService: NotifyService,
    private location: Location
  ) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(map => {
      this.roleId = Number.parseInt(map.get('id'), 10);
      this.loadAllRolesAndPermissions();
    });

  }

  onRemovePermissionFromRole(permissionIndex: number) {
    const permission = this.role.permissions[permissionIndex];
    this.role.permissions.splice(permissionIndex, 1);
    this.permissions.push(permission);
    this.orderPermissions();
  }

  onPermissionClick(index: number) {
    this.role.permissions.push(this.permissions[index]);
    this.permissions.splice(index, 1);
    this.orderPermissions();
  }

  private orderPermissions() {
    this.role.permissions.sort((a, b) => {
      return a.name > b.name ? 1 : -1;
    });
    this.permissions.sort((a, b) => {
      return a.name > b.name ? 1 : -1;
    });
  }

  private loadAllRolesAndPermissions() {
    return new Promise((resolve, reject) => {
      this.adminService.getAllRolesAndPermissions().subscribe(result => {
        this.roles = result.roles;
        this.permissions = [];
        const permissions = result.permissions;
        if (this.roleId !== -1) {
          this.role = this.roles.find(rol => rol.id === this.roleId);
          for (const permission of permissions) {
            if (this.role.permissions.findIndex(p => p.id === permission.id) === -1) {
              this.permissions.push(permission);
            }
          }
        } else {
          this.role = new Role();
          this.permissions = permissions;
        }

        this.orderPermissions();
        resolve();
      });
    });
  }

  onSaveRole() {
    this.adminService.createEditRole(this.role).subscribe(result => {
      this.notifyService.showNotification('Role ' + (this.roleId === -1 ? 'created' : 'updated'));
    }, error => {
      this.notifyService.showErrorNotification('Role could not be ' + (this.roleId === -1 ? 'created' : 'updated'));
    });
  }

  onDeleteRole() {
    this.adminService.deleteRole(this.role).subscribe(result => {
      this.notifyService.showNotification('Role deleted');
      this.location.back();
    }, error => {
      this.notifyService.showErrorNotification('Role could not be deleted');
    });
  }

}
