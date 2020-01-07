import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import * as api from '../../../apiUrl.json';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: any;
  constructor(private http: HttpClient) {
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
          }
        })
      )
  }

  register(model) {
    return this.http.post(this.url + "auth/register", model);
  }
}
