import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './signup/signup.component';
// Optionally, if you add a dashboard or any protected route in future:
// import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { BookingsComponent } from './bookings/bookings.component';
import { SlotComponent } from './slot/slot.component';
import { BookingComponent } from './booking/booking.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { SignalrEventComponent } from './signalr-event/signalr-event.component';

export const routes: Routes = [
  { path: '', component: HomeComponent}, // Redirect root to the login page
  { path: 'login', component: LoginComponent},          // Route for the login component
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'signup', component: SignupComponent},          // Route for the signup component
  { path: 'slots', component: SlotComponent, canActivate: [AuthGuard]},
  { path: 'bookings', component: BookingsComponent, canActivate: [AuthGuard]},
  { path: 'booking/:id', component: BookingComponent, canActivate: [AuthGuard]},
  { path: 'invoice/:id', component: InvoiceComponent, canActivate: [AuthGuard]},
  { path: 'notifications', component: SignalrEventComponent, canActivate: [AuthGuard]},
  // Uncomment the below route if you add a protected dashboard component:
  // { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }                     // Wildcard route redirects back to login
];
