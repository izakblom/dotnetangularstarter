import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BackNavComponent } from './back-nav/back-nav.component';
import { ClarityModule } from '@clr/angular';
import { RandCurrencyPipe } from './pipes/rand-currency.pipe';
import { DecimalPipe } from '@angular/common';
import { TickerCardModule } from './ticker-card/ticker-card.module';

@NgModule({
  declarations: [
    BackNavComponent,
    RandCurrencyPipe
  ],
  imports: [
    CommonModule,
    ClarityModule,
    TickerCardModule
  ],
  exports: [
    BackNavComponent,
    RandCurrencyPipe,
    TickerCardModule
  ],
  providers: [
    DecimalPipe
  ]
})
export class UtilitiesModule { }
