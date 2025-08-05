import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

// Pages
import { Home } from './pages/home/home';
import { Products } from './pages/products/products';
import { ProductDetails } from './pages/product-details/product-details';
import { News } from './pages/news/news';
import { NewsDetails } from './pages/news-details/news-details';
import { Contact } from './pages/contact/contact';
import { Cart } from './pages/cart/cart';
import { Checkout } from './pages/checkout/checkout';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Profile } from './pages/profile/profile';
import { OrderHistory } from './pages/order-history/order-history';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'products', component: Products },
  { path: 'products/:id', component: ProductDetails },
  { path: 'news', component: News },
  { path: 'news/:id', component: NewsDetails },
  { path: 'contact', component: Contact },
  { path: 'cart', component: Cart },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'profile', component: Profile, canActivate: [authGuard] },
  { path: 'checkout', component: Checkout, canActivate: [authGuard] },
  { path: 'orders', component: OrderHistory, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
