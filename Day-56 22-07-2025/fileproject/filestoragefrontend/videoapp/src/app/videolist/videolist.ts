import { Component } from '@angular/core';
import { VideoService } from '../services/videoservice';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-videolist',
  imports: [CommonModule],
  templateUrl: './videolist.html',
  styleUrl: './videolist.css'
})
export class Videolist {
  videos: any[] = [];

  constructor(private videoService: VideoService) {}

  ngOnInit(): void {
    this.videoService.getVideos().subscribe(res => {
      this.videos = res as any[];
    });
  }
}
