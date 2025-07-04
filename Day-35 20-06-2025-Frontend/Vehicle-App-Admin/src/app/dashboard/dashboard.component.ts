import { Component, OnInit } from '@angular/core';
import { AgChartsModule } from 'ag-charts-angular';
import { BookingService } from '../services/booking.service';
import { CommonModule } from '@angular/common';
import type { AgChartOptions } from 'ag-charts-community';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  imports: [CommonModule, AgChartsModule]
})
export class DashboardComponent implements OnInit {
  statusChartOptions: AgChartOptions = {};
  mechanicChartOptions: AgChartOptions = {};

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.bookingService.getAll().subscribe(bookings => {
      this.generateStatusChart(bookings);
      this.generateMechanicChart(bookings);
    });
  }

  generateStatusChart(bookings: any[]): void {
    const statusCounts: Record<string, number> = {};
    for (const b of bookings) {
      const status = b.status.toLowerCase();
      statusCounts[status] = (statusCounts[status] || 0) + 1;
    }

    const data = Object.entries(statusCounts).map(([status, count]) => ({
      status,
      count
    }));
    this.statusChartOptions = {
      data: data,
      series: [
        {
          type: 'pie',
          angleKey: 'count',
          calloutLabelKey: 'status',
          sectorLabelKey: 'count',
          sectorLabel: {
              color: 'white',
              fontWeight: 'bold',
          },
        }
      ],
      title: {
        text: 'Bookings Status'
      }
    };
    console.log(this.statusChartOptions);
  }

  generateMechanicChart(bookings: any[]): void {
    const mechanicCounts: Record<string, number> = {};
    for (const b of bookings) {
      const name = b.mechanicName;
      mechanicCounts[name] = (mechanicCounts[name] || 0) + 1;
    }

    const data = Object.entries(mechanicCounts).map(([name, count]) => ({
      mechanic: name,
      count
    }));

    this.mechanicChartOptions = {
      data,
      series: [
        {
          type: 'bar',
          xKey: 'mechanic',
          yKey: 'count',
          fill: '#4F46E5'
        }
      ],
      title: {
        text: 'No. of Bookings by Mechanic'
      },
      axes: [
        { type: 'category', position: 'bottom' },
        { type: 'number', position: 'left' }
      ]
    };
  }
}