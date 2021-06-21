import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'randcurrency'
})
export class RandCurrencyPipe implements PipeTransform {

  constructor(private numberPipe: DecimalPipe) { }

  transform(value: any, args?: any): any {
    return 'R' + this.numberPipe.transform(value, '1.2-2');
  }

}
