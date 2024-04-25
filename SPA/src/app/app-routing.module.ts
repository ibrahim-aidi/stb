import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';
import { NoAuthGuard } from './Services/noauth.guard';
import { AuthGuard } from './Services/auth.guard';
import { AjoutComponent } from './admin/ajout/ajout.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    component: HomeComponent, // Route for the home page
  },
  {
    path: 'login',
    canActivate: [NoAuthGuard],
    component: LoginComponent, // Route for the login page
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'ajout',
        component: AjoutComponent // Route for the admin ajout page
      }
    ]
  },
  {
    path: '**', // Wildcard route for any other routes not defined above
    component: NotFoundComponent, // Route for the 404 Not Found page
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
