import { DynamicFormElement } from './dynamicFormElement';

export class TextBoxDynamicFElement extends DynamicFormElement<string> {
  controlType = 'textbox';
  type: string;
  pattern: string;
  patternValidationMsg: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options['type'] || '';
    this.pattern = options['pattern'];
    this.patternValidationMsg = options['patternValidationMsg']
  }
}
