// src/app/components/weather-dashboard/weather-dashboard.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CitySearchComponent } from '../city-search/city-search';
import { WeatherCardComponent } from '../weather-card/weather-card';
import { WeatherService } from '../../services/weather';

@Component({
  selector: 'app-weather-dashboard',
  standalone: true,
  imports: [CommonModule, CitySearchComponent, WeatherCardComponent],
  templateUrl: "./weather-dashboard.html",
  styleUrl: "./weather-dashboard.css"
})
export class WeatherDashboardComponent {
  historyArray: string[] = [];

  constructor(public weatherService: WeatherService) {
    // Load search history from localStorage.
    const storedHistory = localStorage.getItem('weatherHistory');
    if (storedHistory) {
      this.historyArray = JSON.parse(storedHistory);
    }
  }

  searchFromHistory(city: string): void {
    this.weatherService.searchCity(city);
  }
}
