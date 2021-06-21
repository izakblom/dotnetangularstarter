import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { DynamicFormsModule } from '../dynamic-forms/dynamic-forms.module';
import { DynamicFormDemoComponent } from './dynamic-form-demo/dynamic-form-demo.component';
import { DataTableModule } from '../data-table/data-table.module';
import { TickersModule } from './../shared/tickers/tickers.module';

@NgModule({
  declarations: [HomeComponent, DynamicFormDemoComponent],
  imports: [
    CommonModule,
    MaterialModule,
    MatToolbarModule,
    RouterModule,
    DynamicFormsModule,
    TickersModule
  ]
})
export class HomeModule { }
