import { Permission } from './permission';

export class FeatureGuard {
  constructor(
    public name: string,
    public permissionsAll: Permission[],
    public permissionsAny: Permission[],
    public urlRegex?: string) { }
}
