import { DynamicFormElement } from './dynamicFormElement';

export class CheckBoxDynamicFElement extends DynamicFormElement<boolean> {
  controlType = 'checkbox';

  constructor(options: {} = {},
  ) {
    super(options);

  }
}
