import { DynamicFormElement } from './dynamicFormElement';

export class DropdownDynamicFElement extends DynamicFormElement<string> {
  controlType = 'dropdown';
  options: { key: string, value: string }[] = [];
  optionsFetchAddress?: string;

  constructor(options: {} = {}
  ) {
    super(options);
    this.options = options['options'] || [];
    this.optionsFetchAddress = options['optionsFetchAddress'] || null;
  }
}
