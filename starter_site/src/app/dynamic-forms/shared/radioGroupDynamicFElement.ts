import { DynamicFormElement } from './dynamicFormElement';

export class RadioGroupDynamicFElement extends DynamicFormElement<string> {
  controlType = 'radio';
  options: { key: string, value: string }[] = [];
  optionsFetchAddress?: string;

  constructor(options: {} = {}
  ) {
    super(options);
    this.options = options['options'] || [];
    this.optionsFetchAddress = options['optionsFetchAddress'] || null;
  }
}
