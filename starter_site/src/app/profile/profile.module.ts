import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileSetupComponent } from './profile-setup/profile-setup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ClarityModule } from '@clr/angular';
import { DynamicFormsModule } from '../dynamic-forms/dynamic-forms.module';

@NgModule({
  declarations: [ProfileSetupComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ClarityModule,
    DynamicFormsModule
  ],
})
export class ProfileModule { }
