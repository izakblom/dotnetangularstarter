import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-simple-chart',
  templateUrl: './simple-chart.component.html',
  styleUrls: ['./simple-chart.component.css']
})
export class SimpleChartComponent implements OnInit {
  @Input() title = '';
  @Input() data = [];
  @Input() xAxisLabel = '';
  @Input() yAxisLabel = '';
  @Input() height = null;
  @Input() type = 'LINE';

  view = null;

  // maxY = 1;

  constructor() { }

  ngOnInit() {


    if (this.height) {
      this.view = [2.5 * this.height, this.height];
    }
  }

}
