import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  values = [];


  constructor(
    private userService: UserService,
    private router: Router,
    public authService: AuthService,
  ) { }


  ngOnInit() {


    this.getUserProfile();
  }

  private getUserProfile() {
    this.userService.getProfile().subscribe(result => {

      if (!result.complete) {
        this.router.navigate(['profile']);
      }
    }, error => {
      this.router.navigate(['profile']);
    });
  }

}
