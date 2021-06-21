import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { Ticker } from '../Ticker';
import { TickerTemplate } from '../TickerTemplate';
import { CollectionFilter } from '../TickerFilter';

@Component({
  selector: 'app-ticker-configurator',
  templateUrl: './ticker-configurator.component.html',
  styleUrls: ['./ticker-configurator.component.css']
})
export class TickerConfiguratorComponent implements OnInit, AfterViewInit {

  @Input() templates: TickerTemplate[];
  @Input() ticker: Ticker;

  @Output() save: EventEmitter<any> = new EventEmitter();
  @Output() cancel: EventEmitter<any> = new EventEmitter();

  public templateIds: string[];
  public activeTemplate: string = null;
  public selectedTicker: Ticker = null;

  public smColTypes = [6, 12];
  public smRowTypes = [8];
  public mdColTypes = [6, 12];
  public mdRowTypes = [2, 4, 8];
  public lgColTypes = [3, 4, 6, 8, 12];
  public lgRowTypes = [2, 4];

  constructor() { }

  ngOnInit() {
    this.templateIds = Object.keys(this.templates);
    this.activeTemplate = this.ticker.template.category.name;

    this.selectedTicker = new Ticker(this.ticker.title, this.ticker.color, this.ticker.template);
    this.selectedTicker.lgCols = +this.ticker.lgCols;
    this.selectedTicker.lgRows = +this.ticker.lgRows;
    this.selectedTicker.mdCols = +this.ticker.mdCols;
    this.selectedTicker.mdRows = +this.ticker.mdRows;
    this.selectedTicker.smCols = +this.ticker.smCols;
    this.selectedTicker.smRows = +this.ticker.smRows;
    this.selectedTicker.id = this.ticker.id;
    this.selectedTicker.textColor = this.ticker.textColor;
    this.selectedTicker.filter = this.ticker.filter;
    this.selectedTicker.filter.templateId = this.ticker.template.id;
  }

  ngAfterViewInit() {

  }

  onSave() {

    this.ticker.title = this.selectedTicker.title;
    this.ticker.template = this.selectedTicker.template;
    this.ticker.color = this.selectedTicker.color;
    this.ticker.textColor = this.selectedTicker.textColor;
    this.ticker.lgCols = +this.selectedTicker.lgCols;
    this.ticker.lgRows = +this.selectedTicker.lgRows;
    this.ticker.mdCols = +this.selectedTicker.mdCols;
    this.ticker.mdRows = +this.selectedTicker.mdRows;
    this.ticker.smCols = +this.selectedTicker.smCols;
    this.ticker.smRows = +this.selectedTicker.smRows;
    this.ticker.filter = this.selectedTicker.filter;
    this.ticker.filter.templateId = +this.selectedTicker.template.id;

    this.save.emit(this.ticker);
    // console.log(this.ticker.filter);
  }

  onCancel() {
    this.cancel.emit(null);
  }

  setActiveTemplate(t: string) {
    this.activeTemplate = t;
  }

  setSelectedTickerTemplate(t: TickerTemplate) {
    this.selectedTicker.template = t;
  }

  collectionFilterChanged(filter: CollectionFilter) {

    let index = -1;
    let exists = false;

    for (const f of this.selectedTicker.filter.collectionFilters) {
      if (f.type.id === filter.type.id) {
        exists = true;
        index = this.selectedTicker.filter.collectionFilters.indexOf(f);
      }
    }

    if (!exists) {
      this.selectedTicker.filter.collectionFilters.push(filter);
    } else {
      this.selectedTicker.filter.collectionFilters[index].items = filter.items;
    }
  }

  textFilterChanged(t: any) {
    this.selectedTicker.filter.textFilter = t.filterText;
  }

  dateFilterChanged(t: any) {
    // console.log(t);
    this.selectedTicker.filter.from = t.from;
    this.selectedTicker.filter.to = t.to;
    this.selectedTicker.filter.selectedDateRangeFilter = null;

    if (t.dateFilter != null) {
      this.selectedTicker.filter.selectedDateRangeFilter = t.dateFilter.id;
    }
  }
}


