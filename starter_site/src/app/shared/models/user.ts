import { Permission } from './permission';

export class User {
  public firstName: string;
  public lastName: string;
  public email: string;
  public id: string;
  public mobile: string;
  public isActive: boolean;
  public permissions: Permission[];
}
