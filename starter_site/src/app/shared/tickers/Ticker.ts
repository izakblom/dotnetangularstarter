import { TickerTemplate } from './TickerTemplate';
import { TickerFilter } from './TickerFilter';
import { Injector } from '@angular/core';
import { TickerService } from './ticker.service';
import { ApiService } from 'src/app/services/api.service';

export class Ticker {

    public id;
    // grid system is 12 cols
    public lgCols = 3;
    public lgRows = 2;

    public mdCols = 6;
    public mdRows = 4;

    public smCols = 12;
    public smRows = 8;

    public title = '';
    // https://www.npmjs.com/package/ngx-color-picker
    // use this snippet
    // <span [style.color]="color"
    //   [cpPosition]="'bottom'"
    //   [cpPositionOffset]="'50%'"
    //   [cpPositionRelativeToArrow]="true"
    //   [(colorPicker)]="color">Change me!</span>

    public color = '';
    public textColor = '#000000';

    public titleSize = '1em';
    public valueSize = '3.6em';
    public subtextSize = '0.7em';
    public displayValue = '';

    // based on the configured template
    public template: TickerTemplate = null;

    public filter: TickerFilter = null;

    public loading = false;
    public lastRefresh: Date = null;

    constructor(_title: string, _color: string, _template: TickerTemplate) {

        this.title = _title;
        this.color = _color;
        this.id = (this.generate_random_string(10) + this.generate_random_number());
        this.template = _template;
        this.displayValue = '';
        this.filter = new TickerFilter();
        this.filter.selectedTicker = this.template.id;

    }

    generate_random_string(string_length) {
        let random_string = '';
        let random_ascii;
        const ascii_low = 65;
        const ascii_high = 90;

        for (let i = 0; i < string_length; i++) {
            random_ascii = Math.floor((Math.random() * (ascii_high - ascii_low)) + ascii_low);
            random_string += String.fromCharCode(random_ascii);
        }
        return random_string;
    }

    generate_random_number() {
        const num_low = 1;
        const num_high = 9;
        return Math.floor((Math.random() * (num_high - num_low)) + num_low);
    }

    refresh(value: string) {
        this.displayValue = value;
        this.lastRefresh = new Date();
    }

    toggleLoading(loading: boolean) {
        this.loading = loading;
    }

    getDataObject() {
        return {
            id: this.id,
            lgCols: this.lgCols,
            lgRows: this.lgRows,
            mdCols: this.mdCols,
            mdRows: this.mdRows,
            smCols: this.smCols,
            smRows: this.smRows,
            title: this.title,
            color: this.color,
            textColor: this.textColor,
            titleSize: this.titleSize,
            valueSize: this.valueSize,
            subtextSize: this.subtextSize,
            displayValue: '',
            loading: false,
            template: this.template,
            filter: this.filter,

        };
    }

    buildTickerFromObject(d: any) {
        this.id = d.id;
        this.lgCols = d.lgCols;
        this.lgRows = d.lgRows;
        this.mdCols = d.mdCols;
        this.mdRows = d.mdRows;
        this.smCols = d.smCols;
        this.smRows = d.smRows;
        this.title = d.title;
        this.color = d.color;
        this.textColor = d.textColor;
        this.titleSize = d.titleSize;
        this.valueSize = d.valueSize;
        this.subtextSize = d.subtextSize;
        this.displayValue = '';
        this.loading = false;
        this.template = d.template;
        this.filter = d.filter;
    }
}
