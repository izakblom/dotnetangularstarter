import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SimpleChartComponent } from './simple-chart/simple-chart.component';
import { ClarityModule } from '@clr/angular';

import { NgxChartsModule } from '@swimlane/ngx-charts';

@NgModule({
  declarations: [SimpleChartComponent],
  imports: [
    CommonModule,
    ClarityModule,
    NgxChartsModule
  ],
  exports: [
    SimpleChartComponent
  ]
})
export class ChartsModule { }
