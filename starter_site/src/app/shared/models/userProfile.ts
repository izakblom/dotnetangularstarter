import { DynamicFormElement } from 'src/app/dynamic-forms/shared/dynamicFormElement';
import { Permission } from './permission';

export class UserProfile {
  constructor(
    public firstName = '',
    public lastName = '',
    public email = '',
    public mobileNumber = '',
    public idNumber = '',
    public jwtId = '',
    public validated = false,
    public complete = false,
    public permissions: Permission[] = [],
    public dynamicForm: { elements: DynamicFormElement<any>[] } = null,
    public id = ''
  ) { }
}

