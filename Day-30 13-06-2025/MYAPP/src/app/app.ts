import { Component } from '@angular/core';

import { Products } from './products/products';
import { Recipes } from './recipes/recipes';
import { Menu } from './menu/menu';
import { Login } from './login/login';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Products, Recipes, Menu, Login]
})
export class App {
  protected title = 'myApp';
}