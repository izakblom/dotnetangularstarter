import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DynamicFormService } from '../../dynamic-form.service';
import { DynamicFormElement } from '../../shared/dynamicFormElement';
import { FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.css'],
  providers: [DynamicFormService]
})
export class DynamicFormComponent implements OnInit {
  // Contains the form control specifications from which the form is generated
  @Input() formElements: DynamicFormElement<any>[] = [];
  // The text to display on the submit button
  @Input() submitText = 'Submit';
  // Optional URL from which to load form definition. Omit formElements when specifying this.
  // Append query parameters with ? and & separators
  @Input() apiURL: string;
  // Optional URL to which form may be submitted. If specified, the formSubmit event will not be fired
  @Input() submitAPIURL: string;
  // Triggered on form submit, contains the form value object
  @Output() formSubmit = new EventEmitter<any>();
  // Triggered if submitAPIURL was specified and form submission results in an api error
  @Output() submitAPIError = new EventEmitter<any>();
  // Triggered if submitAPIURL was specified and form submission result in a success response
  @Output() submitAPIResult = new EventEmitter<any>();

  @Input() title = '';

  form: FormGroup;

  constructor(private dfService: DynamicFormService, private apiService: ApiService) { }

  ngOnInit() {
    if (this.apiURL) {
      // load form definition from url
      this.loadFormDefinitionFromUrl();
    } else if (this.formElements) {
      this.form = this.dfService.toFormGroup(this.formElements);
    } else {
      // console.log('DynamicFormComponent error, specify either formElements or apiURL as input!');
    }

  }

  private loadFormDefinitionFromUrl() {
    const params = [];
    let url = this.apiURL;
    if (this.apiURL.includes('?')) {
      const pms = this.apiURL.split('?')[1];
      if (pms.includes('&')) {
        const pairs = pms.split('&');
        for (const pair of pairs) {
          const operands = pair.split('=');
          params.push({ value: operands[1], key: operands[0] });
        }
      } else {
        const operands = pms.split('=');
        params.push({ value: operands[1], key: operands[0] });
      }
      url = this.apiURL.split('?')[0];
    }
    this.apiService.get(url, params, null, true).subscribe(result => {
      this.formElements = result.elements;
      this.form = this.dfService.toFormGroup(this.formElements);
    }, error => {
      console.error('error in DynamicFormComponent: ', error);
    });
  }

  onSubmit() {
    // patch the form value with hidden field values
    for (const formel of this.formElements) {
      if (!formel.visible) {
        this.form.value[formel.key] = formel.value;
      }
    }
    // patch null values with empty strings
    for (const prop in this.form.value) {
      if (this.form.value.hasOwnProperty(prop)) {
        if (this.form.value[prop] === null) {
          this.form.value[prop] = '';
        }
      }
    }
    if (this.submitAPIURL) {
      this.apiService.post(this.submitAPIURL, this.form.value, null, true).subscribe(result => {
        this.submitAPIResult.emit(result);
      }, error => {
        this.submitAPIError.emit(error);
      });
    } else {
      this.formSubmit.emit(this.form.value);
    }
  }
}
