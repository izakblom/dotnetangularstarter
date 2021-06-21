import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-ticker-card',
  templateUrl: './ticker-card.component.html',
  styleUrls: ['./ticker-card.component.css']
})
export class TickerCardComponent implements OnInit {
  @Input() title = '';
  @Input() value = '';
  @Input() subValue = '';
  @Input() refreshAPIRoute = '';
  @Input() enableClick = false;
  @Output() tickerClick = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  onTickerClick() {
    if (this.enableClick) {
      this.tickerClick.emit(true);
    }
  }

}
