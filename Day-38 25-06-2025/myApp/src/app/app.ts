import { Component } from '@angular/core';
import { First } from "./first/first";

// import { Products } from './products/products';
import { Recipes } from './recipes/recipes';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [ Recipes]
})
export class App {
  protected title = 'myApp';
}
