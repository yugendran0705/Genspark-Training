// src/app/components/city-search/city-search.component.ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WeatherService } from '../../services/weather';

@Component({
  selector: 'app-city-search',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './city-search.html',
  styleUrl: './city-search.css'
})
export class CitySearchComponent {
  city: string = '';

  constructor(public weatherService: WeatherService) {}

  onSearch(): void {
    if (this.city.trim()) {
      this.weatherService.searchCity(this.city);
      this.city = ''; // Optionally clear input after search
    }
  }
}
