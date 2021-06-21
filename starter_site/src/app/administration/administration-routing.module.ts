import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '../shared/guard/permission.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RolesEditComponent } from './roles/roles-edit/roles-edit.component';
import { RolesListComponent } from './roles/roles-list/roles-list.component';
import { RolesComponent } from './roles/roles.component';
import { UserDetailsComponent } from './users/user-details/user-details.component';
import { UsersListComponent } from './users/users-list/users-list.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: 'users',
    component: UsersComponent,
    canActivate: [PermissionGuard],
    children: [
      {
        path: '',
        component: UsersListComponent,
        canActivate: [PermissionGuard]
      },
      {
        path: ':id',
        component: UserDetailsComponent,
        canActivate: [PermissionGuard]
      },
    ]
  },
  {
    path: 'roles',
    component: RolesComponent,
    canActivate: [PermissionGuard],
    children: [
      {
        path: '',
        component: RolesListComponent,
        canActivate: [PermissionGuard]
      },
      {
        path: ':id',
        component: RolesEditComponent,
        canActivate: [PermissionGuard]
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrationRoutingModule { }
