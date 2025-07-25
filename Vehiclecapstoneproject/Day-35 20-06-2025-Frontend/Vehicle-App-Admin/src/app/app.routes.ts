import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { BookingsComponent } from './bookings/bookings.component';
import { SlotComponent } from './slot/slot.component';
import { BookingComponent } from './booking/booking.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { InvoicesComponent } from './invoices/invoices.component';

export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
  { path: '', component: LoginComponent},
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  { path: 'slots', component: SlotComponent, canActivate: [AuthGuard]},
  { path: 'bookings', component: BookingsComponent, canActivate: [AuthGuard]},
  { path: 'booking/:id', component: BookingComponent, canActivate: [AuthGuard]},
  { path: 'invoices', component: InvoicesComponent, canActivate: [AuthGuard]},
  { path: 'invoice/:id', component: InvoiceComponent, canActivate: [AuthGuard]},
  { path: '**', redirectTo: '' }
];
