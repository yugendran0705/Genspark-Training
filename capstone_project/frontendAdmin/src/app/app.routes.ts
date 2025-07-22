import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';
import { About } from './about/about';
import { AuthGuard } from './auth-guard';
import { Event } from './event/event';
import { Register } from './register/register';
import { Profile } from './profile/profile';
import { Add } from './add/add';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
    {path:"home",component:Home,canActivate:[AuthGuard]},
    {path:"",component:Dashboard,canActivate:[AuthGuard]},
    {path:"login",component:Login},
    {path:"about",component:About},
    {path:"event/:id",component:Event},
    {path:"register",component:Register},
    {path:"add",component:Add},
    {path:"profile",component:Profile,canActivate:[AuthGuard]},
    
];
