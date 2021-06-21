import { Ticker } from './Ticker';
import { TickerService } from './ticker.service';

export class Tab {

  public tickers: Ticker[];
  public title = '';
  public active = false;

  public activeTicker: Ticker = null;

  public timer: any = null;

  constructor(_title: string, _tickers: Ticker[], _active: boolean, public _tickerService: TickerService) {
    this.title = _title;
    this.tickers = _tickers;

    if (_active) {
      this.activate();
    }
  }

  addTicker(_template: any): string {

    const ticker = new Ticker('new ticker ' + (this.tickers.length + 1), '#ffffff', _template);
    ticker.filter.templateId = _template.id;
    // console.log('On add ticker');
    // console.log(ticker);
    this.tickers.unshift(ticker);

    return '';
  }

  deleteTicker(ticker: Ticker): string {

    const index = this.tickers.indexOf(ticker);

    if (index === -1) {
      return 'Ticker not found!';
    }

    this.tickers.splice(index, 1);

    return '';

  }

  configureTicker(ticker: Ticker) {
    this.activeTicker = new Ticker(ticker.title, ticker.color, ticker.template);

    this.activeTicker.id = ticker.id;
    this.activeTicker.lgCols = +ticker.lgCols;
    this.activeTicker.lgRows = +ticker.lgRows;
    this.activeTicker.mdCols = +ticker.mdCols;
    this.activeTicker.mdRows = +ticker.mdRows;
    this.activeTicker.smCols = +ticker.smCols;
    this.activeTicker.smRows = +ticker.smRows;
    this.activeTicker.textColor = ticker.textColor;
    this.activeTicker.filter = ticker.filter;

  }

  toggleTickerLoading(ticker: Ticker, loading: boolean) {
    const index = this.tickers.indexOf(ticker);

    this.tickers[index].toggleLoading(loading);
  }

  refreshTicker(ticker: Ticker, value: string) {
    const index = this.tickers.indexOf(ticker);

    this.tickers[index].refresh(value);
  }

  cancelTickerConfiguration() {
    this.activeTicker = null;
  }

  saveTickerConfiguration(ticker: Ticker): string {

    let index = -1;

    for (let x = 0; x < this.tickers.length; x++) {

      if (this.tickers[x].id === ticker.id) {
        index = x;
      }
    }

    if (index !== -1) {
      this.tickers[index] = ticker;
    } else {
      return 'Ticker not found!';
    }

    this.activeTicker = null;

    return '';
  }

  configuringTicker(): boolean {
    return this.activeTicker != null;
  }

  getDataObject() {

    const res = { title: '', active: false, tickers: [] };

    res.title = this.title;
    res.active = this.active;

    for (let x = 0; x < this.tickers.length; x++) {
      res.tickers.push(this.tickers[x].getDataObject());
    }

    return res;
  }

  buildTabFromData(d: any) {

    this.title = d.title;
    this.active = d.active;

    this.tickers = [];
    for (let x = 0; x < d.tickers.length; x++) {
      const t = new Ticker(d.tickers[x].title, d.tickers[x].color, d.tickers[x].template);
      t.buildTickerFromObject(d.tickers[x]);
      this.tickers.push(t);
    }
  }

  activate() {
    this.scheduleFetch();
    this.active = true;
  }

  deactivate() {
    clearInterval(this.timer);
    this.active = false;
  }

  scheduleFetch() {

    this.timer = setInterval(function () {
      this.fetch();
    }.bind(this), (1000 * 60 * 5)); // every 5 minutes

    this.fetch();
  }

  fetch() {

    for (let x = 0; x < this.tickers.length; x++) {

      this.toggleTickerLoading(this.tickers[x], true);

      this._tickerService.getTickerData(this.tickers[x].filter)
        .subscribe(
          (resp: any) => {
            this.refreshTicker(this.tickers[x], resp.count);
            this.toggleTickerLoading(this.tickers[x], false);
          },
          (err: any) => {
            this.toggleTickerLoading(this.tickers[x], false);
          }
        );
    }

  }

}
