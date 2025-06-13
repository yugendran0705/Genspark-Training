import { Component } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
import { WeatherDashboardComponent } from './components/weather-dashboard/weather-dashboard';

@Component({
  selector: 'app-root',
  imports: [WeatherDashboardComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-dashboard';
}
