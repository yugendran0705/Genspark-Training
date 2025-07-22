import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';
import { History } from './history/history';
import { AuthGuard } from './auth-guard';
import { Event } from './event/event';
import { Register } from './register/register';
import { Confirmbooking } from './confirmbooking/confirmbooking';
import { Profile } from './profile/profile';

export const routes: Routes = [
    {path:"",component:Home},
    {path:"login",component:Login},
    {path:"history",component:History,canActivate:[AuthGuard]},
    {path:"event/:id",component:Event},
    {path:"confirmbooking/:id",component:Confirmbooking},
    {path:"register",component:Register},
    {path:"profile",component:Profile}
];
