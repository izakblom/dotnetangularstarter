import { DynamicFormElement } from './dynamicFormElement';

export class TextAreaDynamicFElement extends DynamicFormElement<string> {
  controlType = 'textarea';
  minLength = 0;
  maxLength = 400;

  constructor(options: {} = {}) {
    super(options);
    this.minLength = options['minLength'];
    this.maxLength = options['maxLength'];
  }
}
