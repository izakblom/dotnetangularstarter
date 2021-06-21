import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { TableDefinition } from '../data-table/models/tableDefinition';
import { UsersDataTableFilter } from '../shared/models/usersDataTableFilter';
import { User } from '../shared/models/user';
import { Role } from '../shared/models/role';
import { Permission } from '../shared/models/permission';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  readonly API_BASE = 'api/admin';

  constructor(private apiService: ApiService) { }

  getUsersByFilter(filter = '', sortColumn = null,
    sortDirection = 'asc', pageIndex = 0, pageSize = 3): Observable<TableDefinition> {
    return this.apiService.post(this.API_BASE + '/GetUsersByFilter',
      new UsersDataTableFilter(pageIndex, pageSize, sortColumn, sortDirection), null, true);
  }

  getUser(userId: string): Observable<User> {
    return this.apiService.get(this.API_BASE + '/GetUser', [{ key: 'id', value: userId }], null, true);
  }

  getAllRolesAndPermissions(): Observable<{ roles: Role[], permissions: Permission[] }> {
    return this.apiService.get(this.API_BASE + '/GetAllRolesAndPermissions', null, null, true);
  }

  assignRevokePermission(userId: string, permissionId: number): Observable<Permission[]> {
    return this.apiService.post(this.API_BASE + '/AssignRevokePermission', { userId: userId, permissionId: permissionId }, null, true);
  }

  assignRevokeRole(userId: string, roleId: number): Observable<Permission[]> {
    return this.apiService.post(this.API_BASE + '/AssignRevokeRole', { userId: userId, roleId: roleId }, null, true);
  }

  enableDisableUser(userId) {
    return this.apiService.post(this.API_BASE + '/EnableDisableUserAccount', { userId: userId }, null, true);
  }

  createEditRole(role: Role) {
    return this.apiService.post(this.API_BASE + '/CreateEditRole', role, null, true);
  }

  deleteRole(role: Role) {
    return this.apiService.post(this.API_BASE + '/DeleteRole', role, null, true);
  }


}
