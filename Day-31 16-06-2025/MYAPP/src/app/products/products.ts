import { Component } from '@angular/core';
import { Input } from '@angular/core';
import { ProductModel } from '../models/productmodel';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Product {
  @Input() product:ProductModel|null = new ProductModel();
}