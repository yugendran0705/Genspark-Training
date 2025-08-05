import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar';
import { Footer } from './components/footer/footer';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'shop-frontend';
}
