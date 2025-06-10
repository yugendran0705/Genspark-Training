import { Component } from '@angular/core';
import { First } from "./first/first";
import { Customer } from "./customer/customer";
import { Products } from './products/products';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [First, Customer, Products]
})
export class App {
  protected title = 'myApp';
}