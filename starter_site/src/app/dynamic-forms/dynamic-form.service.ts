import { Injectable } from '@angular/core';
import { DynamicFormElement } from './shared/dynamicFormElement';
import { FormControl, Validators, FormGroup } from '@angular/forms';

/**
 * Generates a dynamic FormGroup specification
 */
@Injectable({
  providedIn: 'root'
})
export class DynamicFormService {
  constructor() { }

  /**
   * Generates a FormGroup specification from the provided array of form element specifications
   * which contains default values, validations etc.
   */
  toFormGroup(formElements: DynamicFormElement<any>[]) {
    const group: any = {};

    formElements.forEach(formElement => {
      const validators = [];
      if (formElement.required) {
        validators.push(Validators.required);
      }
      if (formElement.pattern) {
        // console.log('setting pattern validator');

        validators.push(Validators.pattern(formElement.pattern));
      }
      if (formElement.minLength) {
        validators.push(Validators.minLength(formElement.minLength));
      }
      if (formElement.maxLength) {
        validators.push(Validators.maxLength(formElement.maxLength));
      }
      let value = formElement.value;
      if (formElement.controlType === 'checkbox') {
        value = formElement.value === 'True' ? true : false;
      }
      group[formElement.key] = new FormControl({ value: value, disabled: formElement.disabled }, validators);
    });
    return new FormGroup(group);
  }
}
