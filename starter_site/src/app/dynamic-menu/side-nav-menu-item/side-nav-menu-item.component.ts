import { Component, OnInit, Input } from '@angular/core';
import { NavItem } from 'src/app/shared/models/navItem';

@Component({
  selector: 'app-side-nav-menu-item',
  templateUrl: './side-nav-menu-item.component.html',
  styleUrls: ['./side-nav-menu-item.component.css']
})
export class SideNavMenuItemComponent implements OnInit {
  @Input() navItem: NavItem;

  constructor() { }

  ngOnInit() {

  }

}
