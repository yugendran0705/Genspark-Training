import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-products',
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
  products: any[] = [
    { id: 1, name: 'Product 1', price: 100, imageUrl: './favicon.ico', count: 0 },
    { id: 2, name: 'Product 2', price: 200, imageUrl: './favicon.ico', count: 0 },
    { id: 3, name: 'Product 3', price: 300, imageUrl: './favicon.ico', count: 0 },
  ];

  constructor() {
  }

  addToCart(product: any) {
    product.count++;
  }

}
