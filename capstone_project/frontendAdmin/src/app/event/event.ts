import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventpageService } from '../services/eventpage-service';
import { CommonModule } from '@angular/common';
import { HomepageService } from '../services/homepage-service';
import { FormsModule, NgForm } from '@angular/forms';
//@ts-ignore
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './event.html',
  styleUrl: './event.css'
})
export class Event implements OnInit {
  id: string = "";
  event: any = {};
  oldtitle: any = ""
  showToast: boolean = false;
  showDeleteToast: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eventservice: EventpageService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.eventservice.geteventbyid(Number(this.id)).subscribe((data) => {
        this.event = data;
        this.oldtitle = data.title;
        this.event.date = this.formatDateTimeLocal(this.event.date);
      });
    });
  }

  formatDateTimeLocal(dateStr: string): string {
    const date = new Date(dateStr);
    const offset = date.getTimezoneOffset();
    const local = new Date(date.getTime() - offset * 60000);
    return local.toISOString().slice(0, 16);
  }

  updateEvent(form: NgForm) {
    if (form.invalid) return;

    const payload = {
      title: this.event.title,
      description: this.event.description,
      date: new Date(this.event.date).toISOString(), 
      price: this.event.price,
      address: this.event.address,
      city: this.event.city,
      tags: this.event.tags || [], 
      context: this.event.context,
      ticketcount: this.event.ticketcount,
      imageurl: this.event.imageurl,
      categoryName: this.event.category?.name || '' 
    };

    // console.log("Sending updated payload to backend:", payload);
    this.eventservice.updateEvent(payload, this.oldtitle).subscribe({
      next: (data: any) => {
        console.log(data);
        this.showToast = true;

        setTimeout(() => {
          this.showToast = false;
          this.router.navigate(['/']);
        }, 2000);
      },
      error: (err) => { console.log(err) }
    });

  }

  gotohome() {
    this.router.navigate(['/']);
  }
  closeToast() {
    this.showToast = false;
  }

  closeDeleteToast() {
    this.showDeleteToast = false;

  }

  openCancelModal(ticketId: number) {
    const modalEl = document.getElementById('cancelModal');
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();
  }

  confirmCancel() {

    console.log("cancel-", this.event.id)
    this.eventservice.deleteEvent(this.event.title).subscribe({
      next: (data: any) => {
        this.showDeleteToast=true;
        console.log("deleted-", data)
        const modalEl = document.getElementById('cancelModal');
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal?.hide();
        setTimeout(() => {
          this.showDeleteToast = false;
          this.router.navigate(['/']);
        }, 2000);
      },
      error: (err) => {
        console.log(err);
      }
    })

  }
}
