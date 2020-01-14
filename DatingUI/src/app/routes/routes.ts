import { Routes } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { MemberListComponent } from '../member-list/member-list.component';
import { ListsComponent } from '../lists/lists.component';
import { MessageComponent } from '../message/message.component';
import { AuthGuard } from '../guards/auth.guard';
export const appRoutes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      { path: "members", component: MemberListComponent },
      { path: "lists", component: ListsComponent },
      { path: "message", component: MessageComponent },
    ]
  },
  { path: "**", redirectTo: "", pathMatch: "full" },

]
