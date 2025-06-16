// import { Routes } from '@angular/router';
// // import { First } from './first/first';
// import { Login } from './login/login';
// import { Products } from './products/products';

// export const routes: Routes = [
//     // {path:'home',component:First},
//     {path:'login',component:Login},
//     {path:'products',component:Products}
// ];

import { Routes } from '@angular/router';
import { Home } from './home/home';
import { About } from './about/about';

export const routes: Routes = [
  {path:'',component:Home},
  {path:'about',component:About}
];