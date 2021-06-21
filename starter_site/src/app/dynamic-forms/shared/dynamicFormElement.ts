
/**
 * Specifies the creation a form control element of any input type
 * The specification will be used to create formControls for the form
 *
 * Note: When specifying controlType as 'date', specify value as Date.toLocaleDateString() and specify T as type string
 */
export class DynamicFormElement<T> {
  /** Default value for form control element
 * Note: When specifying controlType as 'date', specify value as Date.toLocaleDateString() and specify T as type string
 */
  value: T;
  // Form control variable name
  key: string;
  // Label to display for the form control
  label: string;
  // Specifies required attribute for the form control
  required: boolean;
  // Specifies disabled attribute for the form control
  disabled: boolean;
  // Specifies if the input control should be visible
  visible: boolean;
  // Specifies the type of control, possible values are 'textbox', 'textarea', 'dropdown', 'radio', 'date', 'checkbox'
  controlType: string;
  // Specifies regex pattern to use for form validation
  pattern: string;
  // Specifies the validation message to display on patter validation failure
  patternValidationMsg: string;
  // Specifies minLength formControl validator property (if applicable to controlType)
  minLength: number;
  // Specifies maxLength formControl validator property (if applicable to controlType)
  maxLength: number;
  // For controls like dropdown and radio this contains the available options to select
  options?: { key: string, value: string }[];
  // If options for dropdown or radio are not explicitly set, a url can be set from which the options may be retrieved
  optionsFetchAddress?: string;
  // Specifies input element type attribute on textbox controlType
  type: string;

  constructor(options: {
    value?: T,
    key?: string,
    label?: string,
    required?: boolean,
    disabled?: boolean,
    visible?: boolean,
    order?: number,
    controlType?: string,
    pattern?: string,
    patternValidationMsg?: string,
    minLength?: number,
    maxLength?: number,
    type?: string,
    options?: { key: string, value: string }[],
    optionsFetchAddress?: string
  } = {},
  ) {
    this.value = options.value;
    this.key = options.key || '';
    this.label = options.label || '';
    this.required = !!options.required;
    this.visible = !!options.visible;
    this.disabled = !!options.disabled;
    this.controlType = options.controlType || '';
    this.pattern = options.pattern || '';
    this.patternValidationMsg = options.patternValidationMsg || '';
    this.minLength = options.minLength || 0;
    this.maxLength = options.maxLength || null;
    this.options = options.options || null;
    this.type = options.type || '';
    this.optionsFetchAddress = options.optionsFetchAddress || null;
  }
}
