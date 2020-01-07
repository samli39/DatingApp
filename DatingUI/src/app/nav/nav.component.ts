import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.auth.login(this.model).subscribe(data => {
      console.log(data);
    }, error => {
      console.log("error")
    })
  }

  loggedIn() {
    const token = localStorage.getItem("token");

    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
  }
}
