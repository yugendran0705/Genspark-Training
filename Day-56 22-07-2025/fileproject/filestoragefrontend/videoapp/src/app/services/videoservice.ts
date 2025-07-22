import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class VideoService {
  private baseUrl = 'http://localhost:5172/api/TrainingVideo';

  constructor(private http: HttpClient) {}

  uploadVideo(formData: FormData) {
    return this.http.post(this.baseUrl + '/upload', formData);
  }

  getVideos() {
    return this.http.get(this.baseUrl);
  }
}
