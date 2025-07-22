import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EventpageService } from '../services/eventpage-service';
import { HomepageService } from '../services/homepage-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add.html',
  styleUrl: './add.css',
})
export class Add implements OnInit {
  eventForm!: FormGroup;
  eventId: string = '';
  creatorEmail: string = '';
  showToast: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private eventService: EventpageService
  ) {}

  ngOnInit(): void {
    this.creatorEmail=localStorage.getItem('username')??"";
    this.eventForm = this.fb.group({
          title: ["", Validators.required],
          description: ["", [Validators.required,Validators.minLength(10)]],
          context: ["",[Validators.required,Validators.minLength(10)]],
          address: ["", [Validators.required,Validators.minLength(10)]],
          city: ["", Validators.required],
          categoryName: ["", Validators.required],
          imageurl: ["", Validators.required],
          price: ["", [Validators.required, Validators.min(1)]],
          ticketcount: ["", [Validators.required, Validators.min(1)]],
          date: ["", Validators.required],
        });
    
  }

  onSubmit() {
    if (this.eventForm.valid) {
      const raw = this.eventForm.value;
      const newevent = {
        title: raw.title,
        description: raw.description,
        context: raw.context,
        address: raw.address,
        city: raw.city,
        price: raw.price,
        ticketcount: raw.ticketcount,
        date: new Date(raw.date).toISOString(), 
        tags: [],
        imageurl: raw.imageurl, 
        categoryName: raw.categoryName,
      };

      console.log('Submitting:', newevent);
      this.eventService.addevent(newevent).subscribe({
        next:(data:any)=>{
          console.log(data)
          this.showToast=true;
          setTimeout(() => {
            this.showToast=false;
            this.router.navigate(['/'])
          }, 2000);
        },
        error:(err)=>{
          console.log(err);
        }
      })
      
    }
  }

  closeToast(){
    this.showToast=false;
  }

  gotohome(){
    this.router.navigate(['/'])
  }

  
}
