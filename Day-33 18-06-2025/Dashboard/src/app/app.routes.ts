import { Routes } from '@angular/router';
import { Home } from './home/home';
import { About } from './about/about';
import { Login } from './login/login';
import { AuthGuard } from './auth-guard';
import { Users } from './users/users';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
  {path:'',component:Login},
  {path:'products',component:Home,canActivate:[AuthGuard]},
  {path:'about',component:About},
  {path:'users',component:Users},
  {path:'dashboard',component:Dashboard}
];

