import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { PermissionsService } from 'src/app/services/permissions.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  private readonly VIEW_USER_DETAILS_FEATURE_NAME = 'USER_DETAILS';
  userHasRequiredPermissions = false;


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private permissionsService: PermissionsService
  ) { }

  async ngOnInit() {
    const viewUserDetailsFeat = await this.permissionsService.findFeatureMappingByFeatureName(this.VIEW_USER_DETAILS_FEATURE_NAME);
    if (viewUserDetailsFeat) {
      this.userHasRequiredPermissions = await this.userService.userHasPermissions(viewUserDetailsFeat.permissionsAll);
    }
  }

  onRowClick(row) {
    // console.log('row: ', row);
    this.router.navigate(['./', row.id], { relativeTo: this.route });
  }
}
