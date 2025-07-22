import { Component } from '@angular/core';
import { VideoService } from '../services/videoservice';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-videoupload',
  imports: [FormsModule,CommonModule],
  templateUrl: './videoupload.html',
  styleUrl: './videoupload.css'
})
export class Videoupload {
title = '';
  description = '';
  selectedFile: File | null = null;

  constructor(private videoService: VideoService) {}

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onUpload() {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('title', this.title);
    formData.append('description', this.description);
    formData.append('videoFile', this.selectedFile);

    this.videoService.uploadVideo(formData).subscribe({
      next: (res:any) => alert('Upload successful'),
      error: (err:any) => alert('Upload failed')
    });
  }
}
