import { Component, OnInit } from '@angular/core';
import { AgChartsModule } from 'ag-charts-angular';
import { BookingService } from '../services/booking.service';
import { CommonModule } from '@angular/common';
import type { AgChartOptions } from 'ag-charts-community';
import { UserService } from '../services/user.service';
import { SlotService } from '../services/slot.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  imports: [CommonModule, AgChartsModule]
})
export class DashboardComponent implements OnInit {
  statusChartOptions: AgChartOptions = {};
  mechanicChartOptions: AgChartOptions = {};
  timeChartOptions: AgChartOptions = {};
  slotChartOptions: AgChartOptions = {};
  userRoleChartOptions: AgChartOptions = {};

  constructor(
    private bookingService: BookingService, 
    private userService: UserService,
    private slotService: SlotService
  ) {}

  ngOnInit(): void {
    this.bookingService.getAll().subscribe(bookings => {
      this.generateStatusChart(bookings);
      this.generateMechanicChart(bookings);
    });
    this.userService.getAll().subscribe(users => {
      this.generateUserRoleChart(users);  
    });
    this.slotService.getAll().subscribe(slots => {
      this.generateSlotChart(slots);
      this.generateTimeChart(slots);
    });
  }

  generateSlotChart(slots: any[]): void {
    const statusCounts: Record<string, number> = {};

    for (const s of slots) {
      const status = s.status.toLowerCase();
      statusCounts[status] = (statusCounts[status] || 0) + 1;
    }

    const data = Object.entries(statusCounts).map(([status, count]) => ({
      status,
      count
    }));

    this.slotChartOptions = {
      data,
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
        fills: ['#F0F00F', '#00FF00', '#0000FF', '#FF0000'] 
      }
      ],
      title: {
      text: 'Service Slot Utilization'
      }
    };
  }


  generateUserRoleChart(users: any[]): void {
    const roleCounts: Record<string, number> = {};

    for (const user of users) {
      const role = user.roleName.toLowerCase();
      if (role === 'admin') continue; // Skip admin role
      console.log(role);
      roleCounts[role] = (roleCounts[role] || 0) + 1;
    }

    const data = Object.entries(roleCounts).map(([role, count]) => ({
      role,
      count
    }));

    this.userRoleChartOptions = {
      data,
      series: [
        {
          type: 'pie',
          angleKey: 'count',
          calloutLabelKey: 'role',
          sectorLabelKey: 'count',
          fills: ['#3B82F6', '#10B981']
        }
      ],
      title: { text: 'Users by Role' }
    };
  }


  generateTimeChart(slots: any[]): void {
    const dateCounts: Record<string, number> = {};

    for (const s of slots) {
      console.log(s);
      const date = new Date(s.slotDateTime).toISOString().split('T')[0]; // Extract date in YYYY-MM-DD format
      if (s.status.toLowerCase() !== 'booked') continue; // Only count booked slots
      dateCounts[date] = (dateCounts[date] || 0) + 1;
    }

    const data = Object.entries(dateCounts)
      .map(([date, count]) => ({ date, count }))
      .sort((a, b) => a.date.localeCompare(b.date));

    this.timeChartOptions = {
      data,
      series: [
        {
          type: 'line',
          xKey: 'date',
          yKey: 'count',
          stroke: '#10B981',
          marker: { shape: 'circle', size: 5 }
        }
      ],
      title: { text: 'Bookings Over Time' },
      axes: [
        { type: 'category', position: 'bottom', title: { text: 'Date' } },
        { type: 'number', position: 'left', title: { text: 'Bookings' } }
      ]
    };
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