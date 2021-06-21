import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-back-nav',
  templateUrl: './back-nav.component.html',
  styleUrls: ['./back-nav.component.css']
})
export class BackNavComponent {
  // Optional, specify routeTo relative to current route. Will route to this instead of back location
  @Input() routeTo: string;
  constructor(private location: Location, private router: Router, private route: ActivatedRoute) { }

  onClickBack() {
    if (this.routeTo) {
      this.router.navigate([this.routeTo], { relativeTo: this.route });
    } else {
      this.location.back();
    }
  }

}
