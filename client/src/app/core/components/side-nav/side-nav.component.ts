import { Component, OnInit, Input } from '@angular/core';
import { BreakpointState } from '@angular/cdk/layout/breakpoints-observer';
import { Observable } from 'rxjs';

@Component({
  selector: 'exp-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.css']
})
export class SideNavComponent implements OnInit {

  @Input() isHandset: BreakpointState;

  @Input() title: string;

  constructor() { }

  ngOnInit(): void {
  }

}
