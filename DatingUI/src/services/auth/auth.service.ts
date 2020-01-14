import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as api from '../../../apiUrl.json';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: any;
  decodedToken: any;
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {
    //ef
    this.url = api.efUrl;
    //ado.net
    //this.url = api.adoUrl;
  }

  login(model) {
    return this.http.post(this.url + "auth/login", model)
      .pipe(
        map((user: any) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
          }
        })
      )
  }

  setToken() {
    const token = localStorage.getItem('token');
    if (token) {
      this.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
  register(model) {
    return this.http.post(this.url + "auth/register", model);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
