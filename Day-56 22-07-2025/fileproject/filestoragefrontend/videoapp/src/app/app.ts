import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Videoupload } from './videoupload/videoupload';
import { Videolist } from './videolist/videolist';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Videoupload, Videolist],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'videoapp';
}
