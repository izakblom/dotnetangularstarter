import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';
import { Role } from 'src/app/shared/models/role';
import { Permission } from 'src/app/shared/models/permission';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-roles-list',
  templateUrl: './roles-list.component.html',
  styleUrls: ['./roles-list.component.css']
})
export class RolesListComponent implements OnInit {

  roles: Role[];
  permissions: Permission[];

  constructor(
    private adminService: AdminService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadAllRolesAndPermissions();
  }

  onRemovePermissionFromRole(roleIndex: number, permissionIndex: number) {
    this.roles[roleIndex].permissions.splice(permissionIndex, 1);
  }

  getConnectedRoleLists() {
    const roleIds = [];
    for (let index = 0; index < this.roles.length; index++) {
      roleIds.push('role' + index);
    }
    return roleIds;
  }

  private loadAllRolesAndPermissions() {
    return new Promise((resolve, reject) => {
      this.adminService.getAllRolesAndPermissions().subscribe(result => {
        this.roles = result.roles.sort((a, b) => a.name > b.name ? 1 : -1);
        this.roles.forEach(role => {
          role.permissions.sort((a, b) => a.name > b.name ? 1 : -1);
        });
        this.permissions = result.permissions;
        resolve();
      });
    });
  }

  onRoleConfigureClick(roleIndex: number) {
    if (roleIndex === -1) {
      // new role create
      this.router.navigate(['./', -1], { relativeTo: this.route });
    } else {
      this.router.navigate(['./', this.roles[roleIndex].id], { relativeTo: this.route });
    }
  }
}
