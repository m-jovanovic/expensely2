import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { BreakpointState } from '@angular/cdk/layout/breakpoints-observer';
import { AuthenticationFacade } from '@expensely/core/store';

@Component({
	selector: 'exp-nav-bar',
	templateUrl: './nav-bar.component.html',
	styleUrls: ['./nav-bar.component.css'],
})
export class NavBarComponent implements OnInit {

	@Input() isHandset: BreakpointState;

	@Input() drawer: ElementRef;

	@Input() title: string;

	constructor(private _authenticationFacade: AuthenticationFacade) {}

	ngOnInit(): void {}

	logout(): void {
		this._authenticationFacade.logout();
	}

}
