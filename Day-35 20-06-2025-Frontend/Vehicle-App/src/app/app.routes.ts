import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './signup/signup.component';
// Optionally, if you add a dashboard or any protected route in future:
// import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent}, // Redirect root to the login page
  { path: 'login', component: LoginComponent},          // Route for the login component
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'signup', component: SignupComponent},          // Route for the signup component
  // Uncomment the below route if you add a protected dashboard component:
  // { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }                     // Wildcard route redirects back to login
];
