import { Component, HostListener, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Productservice } from '../services/productservice';
import { ProductModel } from '../models/ProductModel';
import { Product } from '../product/product';
import { CommonModule } from '@angular/common';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, Product, CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  products: ProductModel[] = [];
  searchTerm: string = '';
  searchSubject = new Subject<string>();
  loading: boolean = false;     // for initial search/loading
  isFetching: boolean = false;  // for infinite scroll loading
  limit = 10;
  skip = 0;
  isEnd = false;

  constructor(private productService: Productservice) {}

  ngOnInit() {
    this.loadMoreProducts();

    this.searchSubject
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => {
          this.loading = true;
          this.skip = 0;
          this.isEnd = false;
        }),
        switchMap((query) =>
          this.productService.getPaginatedProducts(this.limit, 0, query)
        ),
        tap(() => (this.loading = false))
      )
      .subscribe((data: any) => {
        this.products = data.products;
        this.skip = this.limit;
        this.isEnd = data.products.length < this.limit;
      });
  }

  handleSearchProducts() {
    this.searchSubject.next(this.searchTerm);
  }

  loadMoreProducts() {
    if (this.isFetching || this.isEnd || this.loading) return;

    this.isFetching = true;
    this.productService
      .getPaginatedProducts(this.limit, this.skip, this.searchTerm)
      .subscribe({
        next: (data: any) => {
          this.products = [...this.products, ...data.products];
          this.skip += this.limit;
          if (data.products.length < this.limit) {
            this.isEnd = true;
          }
          this.isFetching = false;
        },
        error: () => {
          this.isFetching = false;
        },
      });
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const bottom = document.body.offsetHeight - 100;
    if (scrollPosition >= bottom) {
      this.loadMoreProducts();
    }
  }
}
