// src/app/services/weather.service.ts
import { Injectable, signal, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  private readonly API_KEY = '';
  private readonly BASE_URL = 'https://api.openweathermap.org/data/2.5/weather';

  currentCity = signal<string>('');
  weather = signal<any | null>(null);
  error = signal<string>('');

  constructor(private http: HttpClient) {
    effect(() => {
      const city = this.currentCity();
      if (city) {
        this.fetchWeather(city);
      } else {
        this.weather.set(null);
        this.error.set('');
      }
    });

    // Auto-refresh weather data every 5 minutes.
    setInterval(() => {
      if (this.currentCity()) {
        this.fetchWeather(this.currentCity());
      }
    }, 300000); // 300,000 ms = 5 minutes
  }

  searchCity(city: string): void {
    const trimmed = city.trim();
    if (!trimmed) return;
    this.currentCity.set(trimmed);

    // Update search history in localStorage.
    let history = JSON.parse(localStorage.getItem('weatherHistory') || '[]');
    history.unshift(trimmed);
    history = history.slice(0, 5);
    localStorage.setItem('weatherHistory', JSON.stringify(history));
  }

  // Fetch weather data using HttpClient; update weather or error signals.
  fetchWeather(city: string): void {
    const url = `${this.BASE_URL}?q=${city}&appid=${this.API_KEY}&units=metric`;
    this.http.get<any>(url).subscribe({
      next: data => {
        this.weather.set(data);
        this.error.set('');
      },
      error: err => {
        console.error('Error fetching weather:', err);
        this.weather.set(null);
        this.error.set('City not found or there was an API error.');
      }
    });
  }

  // Optionally, you can provide a manual refresh method.
  refresh(): void {
    const city = this.currentCity();
    if (city) {
      this.fetchWeather(city);
    }
  }
}
