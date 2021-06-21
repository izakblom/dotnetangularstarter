import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TickerDashboardComponent } from './ticker-dashboard/ticker-dashboard.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { TickerConfiguratorComponent } from './ticker-configurator/ticker-configurator.component';
import { MaterialModule } from './../../material/material.module';
import { LayoutModule } from '@angular/cdk/layout';
import { ColorPickerModule } from 'ngx-color-picker';
import { ClarityModule } from '@clr/angular';
import { TickerFilterComponent } from './ticker-filter/ticker-filter.component';

@NgModule({
  declarations: [TickerDashboardComponent, TickerConfiguratorComponent, TickerFilterComponent],
  imports: [
    CommonModule,
    DragDropModule,
    MaterialModule,
    ColorPickerModule,
    LayoutModule,
    ClarityModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    TickerDashboardComponent,
    TickerConfiguratorComponent,
    TickerFilterComponent,
    ColorPickerModule
  ]
})
export class TickersModule { }
