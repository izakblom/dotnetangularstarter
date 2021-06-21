export class NavItem {
  constructor(public heading: string, public routerLink: string, public icon: string, public children: NavItem[] = []) { }
}
