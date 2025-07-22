import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EventService } from '../services/event-service';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CarouselModule } from 'primeng/carousel';
import { NgOptimizedImage } from '@angular/common'
import { NotificationService } from '../services/notification-service';

@Component({
  selector: 'app-home',
  imports: [FormsModule, CommonModule, ButtonModule, CarouselModule, NgOptimizedImage],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  searchText = '';
  filters = {
    minPrice: null as number | null,
    maxPrice: null as number | null,
    category: '',
    title: '',
    city: '',
    startDate: null as string | null,
    endDate: null as string | null
  };


  trendingCategories: any[] = [];
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


  allEvents: any = []


  constructor(private eventservice: EventService, private router: Router, private notificationservice: NotificationService) { }

  ngOnInit() {
    this.notificationservice.startConnection();
    this.notificationservice.eventRecieved$.subscribe(() => {
      this.fetchallevents();
    })

    this.fetchallevents();

  }
  fetchallevents() {
    this.eventservice.getallevents().subscribe({
      next: (data) => {
        this.allEvents = data as any;
        this.categories = Array.from(new Set(this.allEvents.map((e: any) => e.category.name)));
        this.cities = Array.from(new Set(this.allEvents.map((e: any) => e.city)));

        const now = new Date();

        
        const categoryCounts: Record<string, { name: string, count: number, image: string }> = {};

        for (const event of this.allEvents) {
          const cat = event.category.name;
          if (!categoryCounts[cat]) {
            categoryCounts[cat] = {
              name: cat,
              count: 0,
              image: event.imageurl || ''
            };
          }
          categoryCounts[cat].count += 1;
        }

        this.trendingCategories = Object.values(categoryCounts)
          .sort((a, b) => b.count - a.count)
          .slice(0, 3)
          .map((cat, index) => ({
            ...cat,
            label: `#${index + 1} Trending – ${cat.name}`
          }));

       
        this.concerts = this.allEvents.filter((e: any) =>
          e.category.name.toLowerCase() === 'concert' && new Date(e.date) > now
        );
        this.Movies = this.allEvents.filter((e: any) =>
          e.category.name.toLowerCase() === 'movie' && new Date(e.date) > now
        );
        this.techevents = this.allEvents.filter((e: any) =>
          e.category.name.toLowerCase() === 'tech' && new Date(e.date) > now
        );
      },
      error: (err) => console.log(err),
      complete: () => console.log("All done")
    });
  }

  filteredEvents() {
    const now = new Date();
    return this.allEvents.filter((e: any) => {
      const eventDate = new Date(e.date);
      const isUpcoming = eventDate > now;

      const matchesMinPrice = this.filters.minPrice == null || e.price >= this.filters.minPrice;
      const matchesMaxPrice = this.filters.maxPrice == null || e.price <= this.filters.maxPrice;
      const matchesCategory = !this.filters.category || e.category.name === this.filters.category;
      const matchesName = !this.filters.title || e.title.toLowerCase().includes(this.filters.title.toLowerCase());
      const matchesSearch = !this.searchText || e.title.toLowerCase().includes(this.searchText.toLowerCase());
      const matchescity = !this.filters.city || e.city === this.filters.city;

      const matchesStartDate = !this.filters.startDate || eventDate >= new Date(this.filters.startDate);
      const matchesEndDate = !this.filters.endDate || eventDate <= new Date(this.filters.endDate);

      return isUpcoming && matchesMinPrice && matchesMaxPrice && matchesCategory && matchesName && matchesSearch && matchescity && matchesStartDate && matchesEndDate;
    });
  }


  handleBookNow(id: any) {
    this.router.navigateByUrl("/event/" + id);

  }
}
