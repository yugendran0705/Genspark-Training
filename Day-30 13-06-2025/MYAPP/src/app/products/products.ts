import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { Product } from "../product/product";
import { CartItem } from '../models/cartItem';



@Component({
  selector: 'app-products',
  standalone: true,
  imports: [Product],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products implements OnInit {
  products: ProductModel[] = [];
  cartItems: CartItem[] = [];
  cartCount: number = 0;

  constructor(private productService: ProductService) {}

  handleAddToCart(productId: Number): void {
    console.log("Handling add to cart - " + productId);
    let flag = false;
    for (let i = 0; i < this.cartItems.length; i++) {
      if (this.cartItems[i].Id === productId) {
        this.cartItems[i].Count++;
        flag = true;
      }
    }
    if (!flag) {
      this.cartItems.push(new CartItem(productId, 1));
    }
    this.cartCount++;
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe({
      next: (data: any) => {
        this.products = data.products as ProductModel[];
      },
      error: (_err: any) => {},
      complete: () => {}
    });
  }
}