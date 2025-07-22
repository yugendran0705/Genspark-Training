import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomepageService } from '../services/homepage-service';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CarouselModule } from 'primeng/carousel';

@Component({
  selector: 'app-home',
  imports: [FormsModule, CommonModule, ButtonModule, CarouselModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  searchText = '';
  filters = {
    category: '',
    title: '',
    city: '',
    isCancelled: ''
  };

  categories: string[] = [];
  cities: string[] = [];
  concerts: any[] = [];
  Movies: any[] = [];
  techevents: any[] = [];

  latestEvents = [
    { title: 'Concert A', description: 'Music concert', category: 'Music', price: 500 },
    { title: 'Tech Talk B', description: 'Tech event', category: 'Tech', price: 200 },
    { title: 'Marathon C', description: 'Sports event', category: 'Sports', price: 100 },
  ];

  // allEvents = [...this.latestEvents, /* more dummy data here */];

  allEvents: any = []

  constructor(private homepageService: HomepageService, private router: Router) { }

  ngOnInit() {
    this.homepageService.getallevents().subscribe(
      {
        next: (data) => {
          this.allEvents = data as any;
          this.categories = Array.from(new Set(this.allEvents.map((e: any) => e.category.name)));
          this.cities = Array.from(new Set(this.allEvents.map((e: any) => e.city)));
          this.concerts = this.allEvents.filter((e: any) => e.category.name.toLowerCase() === 'concert');
          this.Movies = this.allEvents.filter((e: any) => e.category.name.toLowerCase() === 'movie');
          this.techevents = this.allEvents.filter((e: any) => e.category.name.toLowerCase() === 'tech');
          

          console.log(this.categories)
          console.log(this.allEvents)
        },
        error: (err) => {
          console.log(err)
        },
        complete: () => {
          console.log("All done");
        }
      })
  }

  filteredEvents() {
    return this.allEvents.filter((e: any) => {
      const matchesCategory = !this.filters.category || e.category.name === this.filters.category;
      const matchesSearch = !this.searchText || e.title.toLowerCase().includes(this.searchText.toLowerCase());
      const matchescity = !this.filters.city || e.city === this.filters.city;
      const matchesCancelled =!this.filters.isCancelled || String(e.isCancelled) === this.filters.isCancelled;

      return matchesCategory && matchesSearch && matchescity &&matchesCancelled;
    });
  }

  handleBookNow(id: any) {
    this.router.navigateByUrl("/event/" + id);

  }

  addevent() {
    this.router.navigate(['/add'])
  }
}
