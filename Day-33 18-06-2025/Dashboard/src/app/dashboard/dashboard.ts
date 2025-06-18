import { Component, OnInit } from '@angular/core';
// Angular Chart Component
import { AgCharts } from 'ag-charts-angular';
// Chart Options Type Interface
import { AgChartOptions } from 'ag-charts-community';
import { Dashboardservice } from '../services/dashboardservice';

@Component({
  selector: 'app-dashboard',
  imports: [AgCharts],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
// export class Dashboard implements OnInit  {
export class Dashboard{
  users: any[] = [];
  
  // ngOnInit(): void {
  //   this.dashboardservice.getallusers().subscribe({
  //     next: (data) => {
  //       this.users = data.users;
  //     },
  //     error: (err) => {
  //       console.error('Error fetching users:', err);
  //     },
  //   });
  // }

  // Chart Options
  public chartOptions: AgChartOptions;
  public chartOptions1: AgChartOptions;
  constructor(private dashboardservice: Dashboardservice) {

    this.chartOptions = {
      data: [],
      series: [{ 
      type: 'bar', 
      xKey: 'gender', 
      yKey: 'count',
      yName: 'Number of People'
      }],
      axes: [
      { type: 'category', position: 'bottom', title: { text: 'Gender' } },
      { type: 'number', position: 'left', title: { text: 'Count' } }
      ]
    };

    this.chartOptions1 = {
      data: [],
      series: [{ 
      type: 'pie', 
      angleKey: 'count', 
      legendItemKey: 'gender',
      }]
    };

    // After users are loaded, update chart data
    this.dashboardservice.getallusers().subscribe({
      next: (data) => {
      this.users = data.users;

      const genderCounts: { [key: string]: number } = {};
      this.users.forEach(user => {
        const gender = user.gender || 'Unknown';
        genderCounts[gender] = (genderCounts[gender] || 0) + 1;
      });

      this.chartOptions = {
        ...this.chartOptions,
        data: Object.entries(genderCounts).map(([gender, count]) => ({ gender, count }))
      };
      this.chartOptions1 = {
        ...this.chartOptions1,
        data: Object.entries(genderCounts).map(([gender, count]) => ({ gender, count }))
      };
      },
      error: (err) => {
      console.error('Error fetching users:', err);
      },
    });
  }
}
