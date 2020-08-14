import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'exp-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  test: string;
  constructor() {
    this.test = 'test';
   }

  ngOnInit(): void {
  }

}
