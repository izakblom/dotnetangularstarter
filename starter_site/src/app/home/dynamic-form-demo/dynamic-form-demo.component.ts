import { Component, OnInit } from '@angular/core';
import { DynamicFormElement } from 'src/app/dynamic-forms/shared/dynamicFormElement';
import { TextBoxDynamicFElement } from 'src/app/dynamic-forms/shared/textBoxDynamicFElement';
import { DropdownDynamicFElement } from 'src/app/dynamic-forms/shared/dropdownDynamicFElement';
import { CheckBoxDynamicFElement } from 'src/app/dynamic-forms/shared/checkBoxDynamicFElement';
import { RadioGroupDynamicFElement } from 'src/app/dynamic-forms/shared/radioGroupDynamicFElement';
import { TextAreaDynamicFElement } from 'src/app/dynamic-forms/shared/textAreaDynamicFElement';

@Component({
  selector: 'app-dynamic-form-demo',
  templateUrl: './dynamic-form-demo.component.html',
  styleUrls: ['./dynamic-form-demo.component.css']
})
export class DynamicFormDemoComponent implements OnInit {
  formElements: DynamicFormElement<any>[] = [];
  payLoad = '';

  constructor() { }

  ngOnInit() {
    this.formElements.push(new TextBoxDynamicFElement({
      key: 'value1', label: 'Value1',
      required: true,
      type: 'tel',
      pattern: '^([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])(\\d{7})$',
      patternValidationMsg: 'Please enter valid ID number'
    }
    ));
    this.formElements.push(new DropdownDynamicFElement({
      key: 'value2', label: 'Value2',
      required: true, options: [{ key: 'option1', value: 'options1' }, { key: 'option2', value: 'options2' }]
    }
    ));
    this.formElements.push(new CheckBoxDynamicFElement({
      key: 'value3', label: 'Value3',
      required: true
    }));
    this.formElements.push(new RadioGroupDynamicFElement({
      key: 'value4', label: 'Value4',
      required: true, options: [{ key: 'option1', value: 'options1' }, { key: 'option2', value: 'options2' }]
    }
    ));
    this.formElements.push(new TextAreaDynamicFElement({
      key: 'value5', label: 'Value5',
      required: true,
      minLength: 10,
      maxLength: 50
    }
    ));
    this.formElements.push(new DynamicFormElement<string>({
      key: 'value6', label: 'Value6',
      value: new Date().toLocaleDateString(),
      required: true,
      controlType: 'date'
    }));
    this.formElements.push(new DynamicFormElement<string>({
      key: 'value7', label: 'Value7 (fetched)',
      controlType: 'dropdown',
      required: true,
      optionsFetchAddress: 'http://localhost:5000/api/users/GetFormOptionsDemo'
    }));
  }

  onSubmit(value) {
    this.payLoad = JSON.stringify(value);
  }

}
