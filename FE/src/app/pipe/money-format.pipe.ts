import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'moneyFormat',
  pure: false
})
export class MoneyFormatPipe implements PipeTransform {

  constructor() { }

  transform(data: any): any {
    if (data) {
      // xoa dau '.' neu co
      const preFormat = (data + '').replace('.', '').split('');

      const result = [];
      let j = 1;
      for (let i = preFormat.length - 1; i >= 0; i--) {
        const element = preFormat[i];
        result.unshift(element);
        if (j === 3) {
          result.unshift('.');
          j = 0;
        }
        j++;
      }
      if (result.length > 0 && result[0] === '.') {
        result.shift();
      }
      return result.join('');
    } else {
      return data;
    }

  }
}