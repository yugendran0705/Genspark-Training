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
  like: number = 0;  
  dislike: number = 0;
  imageUrl: string = './favicon.ico';
  // className: string = "bi bi-hand-thumbs-up";
  constructor() {
    this.name = "Ramu";
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
