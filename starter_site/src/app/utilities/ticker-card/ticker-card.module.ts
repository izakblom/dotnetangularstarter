import { NgModule } from '@angular/core';
import { TickerCardComponent } from './ticker-card.component';
import { ClarityModule } from '@clr/angular';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    TickerCardComponent
  ],
  imports: [
    ClarityModule,
    CommonModule
  ],
  exports: [
    TickerCardComponent
  ]
})
export class TickerCardModule { }
