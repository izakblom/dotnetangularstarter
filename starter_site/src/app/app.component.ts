import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from './services/auth.service';
import { LocalStorageService } from './services/localStorage.service';
import { NavItem } from './shared/models/navItem';
import { UserService } from './services/user.service';
import { ApiService } from './services/api.service';
import { Router, RouterEvent, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';
import { Subscription } from 'rxjs';
import { NotifyService } from './services/notifyService';
import { PermissionsService } from './services/permissions.service';
import { environment } from 'src/environments/environment';
import { Permission } from './shared/models/permission';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = environment.title + environment.stage;
  signedIn = false;
  email = '';
  showAddContractButton = false;
  topLevelNavItems: NavItem[];
  activeNavItem: NavItem;

  apiRequestPending: boolean;
  routerRouting = false;
  currentBaseRoute = '';
  routerSubscription: Subscription;
  collapsed = false;

  subscriptions: Subscription[] = [];

  constructor(
    private authService: AuthService,
    private localStorageService: LocalStorageService,
    private userService: UserService,
    private apiService: ApiService,
    private router: Router,
    private notifyService: NotifyService,
    private permissionsService: PermissionsService
  ) {
    this.subscriptions.push(this.apiService.requestPending.subscribe(pending => {
      this.apiRequestPending = pending;
    }));
    this.subscriptions.push(this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        // console.log('nav start');
        this.routerRouting = true;
      } else if (event instanceof NavigationEnd || event instanceof NavigationError || event instanceof NavigationCancel) {
        this.routerRouting = false;
        // console.log('nav end');
      }
    }));
  }

  loadUserProfile() {
    return new Promise((resolve, reject) => {
      this.userService.getProfile().subscribe(result => {
        if (result.permissions.find(perm => perm.id === 400) !== undefined) {
          this.showAddContractButton = true;
        } else {
          this.showAddContractButton = false;
        }
        this.authService.profileRetrieved = true;
        resolve();
      }, error => {
        // console.log('loadUserProfile error: ', error);
        resolve();
        if (error.error && error.error.errorCode === 'user/disabled') {
          this.notifyService.showErrorNotification('Your profile has been disabled. Contact support');
          this.authService.SignOut();
        } else {
          this.notifyService.showErrorNotification('Please fill in your profile to continue.');
        }
      });
    });

  }

  ngOnDestroy() {
    for (const sub of this.subscriptions) {
      sub.unsubscribe();
    }
  }

  async ngOnInit() {
    this.showAddContractButton = false;
    this.signedIn = this.authService.isLoggedIn();
    this.authService.AuthChange.subscribe(authState => {
      this.signedIn = authState.authenticated;
      this.email = authState.email;
      if (this.signedIn) {
        this.setupTopLevelNavItems();
        this.loadUserProfile();
      } else {
        // destroy nav menu items
        this.topLevelNavItems = null;
        this.activeNavItem = null;
      }
    });
    this.userService.permissionsChanged.subscribe(() => {
      // Reload navigation menu
      this.setupTopLevelNavItems();
    });

    // Subscribe to route changes so we can sync menu with route url
    this.routerSubscription = this.router.events.subscribe((event: any) => {
      if (event.url) {
        if (event.url.includes('/') && event.url.length > 1) {
          this.currentBaseRoute = '/' + event.url.split('/')[1];
          this.setActiveNavItem();
        } else {
          this.currentBaseRoute = '';
          this.setActiveNavItem();
        }
      }
    });
    if (this.signedIn) {
      await this.loadUserProfile();
      await this.setupTopLevelNavItems();
      await this.permissionsService.getAllFeatureGuards();
      await this.permissionsService.getAllPermissions();
    }

    const userProfile = this.localStorageService.getUserProfile();
    this.email = userProfile ? userProfile.email : null;


  }

  /**
   * Sets the activeNavItem from router url on route path changes, so the correct nav item can
   * be marked as active
   */
  private setActiveNavItem() {
    if (this.topLevelNavItems && (this.currentBaseRoute || this.currentBaseRoute === '')) {
      this.activeNavItem = this.topLevelNavItems.find(tlNI => tlNI.routerLink === this.currentBaseRoute);
    }
  }

  /**
   * Loads the permissioned menu from server
   */
  private setupTopLevelNavItems() {
    return new Promise((resolve, reject) => {
      this.userService.getPermissionedMenu().subscribe(items => {
        // console.log(items);
        this.topLevelNavItems = items;
        this.setActiveNavItem();
        resolve();
      }, error => {
        console.error(error);
        resolve();
      });
    });

  }

  onTopNavClick(index: number) {
    this.activeNavItem = this.topLevelNavItems[index];
  }

  onLogout() {
    // console.log('app.component onLogout');

    this.authService.SignOut();
  }
}


