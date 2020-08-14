import { Component, OnInit } from '@angular/core';
import { BreakpointState } from '@angular/cdk/layout/breakpoints-observer';
import { Observable } from 'rxjs';

import { HandsetStateService } from '@expensely/shared';
import { AuthenticationFacade } from '@expensely/core/store';

@Component({
	selector: 'exp-main-layout',
	templateUrl: './main-layout.component.html',
	styleUrls: ['./main-layout.component.css'],
})
export class MainLayoutComponent implements OnInit {
	isLoggedIn$: Observable<boolean>;
	isHandset$: Observable<BreakpointState>;
	title: string = 'Expensely';

	constructor(
		facade: AuthenticationFacade,
		handsetStateService: HandsetStateService
	) {
		this.isLoggedIn$ = facade.isLoggedIn$;
		this.isHandset$ = handsetStateService.isHandset$;
	}

	ngOnInit(): void {}
}
