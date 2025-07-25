import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from './guards/auth.guard';
import { BookingsComponent } from './bookings/bookings.component';
import { BookingComponent } from './booking/booking.component';

export const routes: Routes = [
  { path: '', component: LoginComponent},
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'bookings', component: BookingsComponent, canActivate: [AuthGuard]},
  { path: 'booking/:id', component: BookingComponent, canActivate: [AuthGuard]},
  { path: '**', redirectTo: '' }
];
