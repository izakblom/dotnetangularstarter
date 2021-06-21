import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { DynamicFormComponent } from './components/dynamic-form/dynamic-form.component';
import { DynamicFormElementComponent } from './components/dynamic-form-element/dynamic-form-element.component';
import { ClarityModule } from '@clr/angular';

@NgModule({
  declarations: [DynamicFormComponent, DynamicFormElementComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ClarityModule
  ],
  exports: [DynamicFormComponent]
})
export class DynamicFormsModule { }
