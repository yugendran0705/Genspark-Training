import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer',
  imports: [FormsModule],
  templateUrl: './customer.html',
  styleUrl: './customer.css'
})
export class Customer {
  name: string = '';
  like: number = 5;  
  dislike: number = 7;
  imageUrl: string = './dp.jpg';
  constructor() {
    this.name = "Chill Guy";
  }
  onButtonClick(uname: string) {
    this.name = uname;
  }
  clickLike() {
    this.like++;
  }
  clickDislike() {
    this.dislike++;
  }
  reset() {
    this.like = 0;
    this.dislike = 0;
  }
}
