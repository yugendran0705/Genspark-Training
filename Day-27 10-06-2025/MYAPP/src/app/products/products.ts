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
    { id: 1, name: 'Dog', price: 100, imageUrl: './images.jpeg', count: 0, quantity: 2 },
    { id: 2, name: 'Cat', price: 200, imageUrl: './cat.png', count: 0, quantity: 1 },
    { id: 3, name: 'Hamster', price: 300, imageUrl: './hamster.jpg', count: 0, quantity: 3 },
  ];

  constructor() {
  }

  addToCart(product: any) {
    if (product.count >= product.quantity) {
      alert(`You can only add ${product.quantity} of ${product.name} to the cart.`);
      return;
    }
    product.count++;
  }

}
