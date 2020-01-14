import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth/auth.service';
import { AlertifyService } from 'src/services/alertify/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public auth: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.auth.login(this.model).subscribe(data => {
      this.alertify.success("login done");
      //redirect to member page adter login
      this.router.navigate(["/members"]);
    }, error => {
      this.alertify.error(error);
    })
  }

  loggedIn() {
    return this.auth.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message("logout");
    //when logout , redirect to home page
    this.router.navigate(["/"]);
  }
}
