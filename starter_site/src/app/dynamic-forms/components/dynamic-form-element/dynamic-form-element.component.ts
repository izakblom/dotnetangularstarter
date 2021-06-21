import { Component, OnInit, Input } from '@angular/core';
import { DynamicFormElement } from '../../shared/dynamicFormElement';
import { FormGroup } from '@angular/forms';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-dynamic-form-element',
  templateUrl: './dynamic-form-element.component.html',
  styleUrls: ['./dynamic-form-element.component.css']
})
export class DynamicFormElementComponent implements OnInit {
  // Contains the specification from which formControl is generated
  @Input() formEl: DynamicFormElement<any>;
  // The FormGroup to which this component's input element's form control belongs
  @Input() form: FormGroup;

  constructor(private apiService: ApiService) { }

  ngOnInit() {
    // console.log('formEl: ', this.formEl || null);

    if (!this.formEl.options && this.formEl.optionsFetchAddress) {

      this.apiService.get(this.formEl.optionsFetchAddress).subscribe(options => {
        this.formEl.options = options;
      }, error => {
        console.error('error loading options: ', error);
      });
    }
  }
}
