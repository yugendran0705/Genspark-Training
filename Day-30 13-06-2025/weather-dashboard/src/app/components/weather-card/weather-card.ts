// src/app/components/weather-card/weather-card.component.ts
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-weather-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weather-card.html',
  styles: './weather-card.css'
})
export class WeatherCardComponent {
  @Input() weather: any;
}
