import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ValuesService } from 'src/services/values.service';
import { NavComponent } from './nav/nav.component';
import { AuthService } from 'src/services/auth/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from 'src/services/error.interceptor.service';
import { AlertifyService } from 'src/services/alertify/alertify.service';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberListComponent } from './member-list/member-list.component';
import { appRoutes } from './routes/routes';
import { ListsComponent } from './lists/lists.component';
import { MessageComponent } from './message/message.component';


function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["https://localhost:44343", "https://localhost:44359"],
        blacklistedRoutes: ["example.com/examplebadroute/"]
      }
    }),
  ],
  providers: [ValuesService, AuthService, ErrorInterceptorProvider, AlertifyService],
  bootstrap: [AppComponent]
})
export class AppModule { }
