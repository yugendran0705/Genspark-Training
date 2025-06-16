import { Component, inject, Input, Output, EventEmitter } from '@angular/core';
import { Productservice } from '../services/productservice';
import { ProductModel } from '../models/product';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
@Input() product:ProductModel|null = new ProductModel();
@Output() addToCart:EventEmitter<Number> = new EventEmitter<Number>();
private productService = inject(Productservice);

handleBuyClick(pid:Number|undefined){
  if(pid)
  {
      this.addToCart.emit(pid);
  }
}
constructor(){
    // this.productService.getProduct(1).subscribe(
    //   {
    //     next:(data)=>{
     
    //       this.product = data as ProductModel;
    //       console.log(this.product)
    //     },
    //     error:(err)=>{
    //       console.log(err)
    //     },
    //     complete:()=>{
    //       console.log("All done");
    //     }
    //   })
}

}