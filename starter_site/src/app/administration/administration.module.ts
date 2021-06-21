import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ClarityModule } from '@clr/angular';
import { DataTableModule } from '../data-table/data-table.module';
import { DynamicFormsModule } from '../dynamic-forms/dynamic-forms.module';
import { ModalModule } from '../shared/modal-module/modal-module.module';
import { UtilitiesModule } from '../utilities/utilities.module';
import { TickersModule } from './../shared/tickers/tickers.module';
import { AdministrationRoutingModule } from './administration-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RolesEditComponent } from './roles/roles-edit/roles-edit.component';
import { RolesListComponent } from './roles/roles-list/roles-list.component';
import { RolesComponent } from './roles/roles.component';
import { UserDetailsComponent } from './users/user-details/user-details.component';
import { UsersListComponent } from './users/users-list/users-list.component';
import { UsersComponent } from './users/users.component';

@NgModule({
  declarations: [
    DashboardComponent, UsersComponent, UserDetailsComponent,
    UsersListComponent, RolesComponent, RolesListComponent,
    RolesEditComponent,
  ],
  imports: [
    DataTableModule,
    CommonModule,
    ClarityModule,
    AdministrationRoutingModule,
    TickersModule,
    UtilitiesModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    DynamicFormsModule,
    ModalModule,
  ]
})
export class AdministrationModule { }
