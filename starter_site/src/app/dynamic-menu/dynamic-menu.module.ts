import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNavMenuItemComponent } from './side-nav-menu-item/side-nav-menu-item.component';
import { RouterModule } from '@angular/router';
import { ClarityModule } from '@clr/angular';

@NgModule({
  declarations: [SideNavMenuItemComponent],
  imports: [
    CommonModule,
    RouterModule,
    ClarityModule
  ],
  exports: [
    SideNavMenuItemComponent
  ]
})
export class DynamicMenuModule { }
