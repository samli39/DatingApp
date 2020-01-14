import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private auth: AuthService) {
  }

  ngOnInit() {
    this.auth.setToken();
  }
}
