import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/services/auth/auth.service';
import { AlertifyService } from 'src/services/alertify/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private auth: AuthService,
    private alertify: AlertifyService
  ) { }

  canActivate(): boolean {
    if (this.auth.loggedIn()) {
      return true;
    }

    this.alertify.error('You should login to access the page');
    this.router.navigate(["/"]);

    return false;


  }

}
