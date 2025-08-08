import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-signalr-event',
  imports: [CommonModule],
  templateUrl: './signalr-event.component.html',
  styleUrl: './signalr-event.component.css'
})
export class SignalrEventComponent implements OnInit, OnDestroy {
  private connection!: signalR.HubConnection;
  messages: any[] = [];

  ngOnInit(): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/eventhub', { withCredentials: true })
      .build();

    this.connection.start()
      .then(() => console.log('Connected to SignalR'))
      .catch(err => console.error('SignalR connection failed', err));

    this.connection.on('ReceiveMessage', (type, SlotDateTime, MechanicName, Status) => {
      this.messages.unshift({
        type,
        SlotDateTime,
        MechanicName,
        Status
      });
    });
  }

  ngOnDestroy(): void {
    this.connection?.stop();
  }
}