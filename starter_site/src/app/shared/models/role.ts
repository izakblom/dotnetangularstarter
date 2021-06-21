import { Permission } from './permission';

export class Role {
  public id: number;
  public name: string;
  public description: string;
  public permissions: Permission[];
  constructor() {
    this.name = '';
    this.description = '';
    this.permissions = [];
    this.id = -1;
  }
}
