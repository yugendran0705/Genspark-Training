import { Component } from '@angular/core';

import { Products } from './products/products';
import { Recipes } from './recipes/recipes';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Products, Recipes]
})
export class App {
  protected title = 'myApp';
}