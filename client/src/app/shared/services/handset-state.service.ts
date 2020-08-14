import { Injectable } from '@angular/core';
import {
	BreakpointState,
	BreakpointObserver,
	Breakpoints,
} from '@angular/cdk/layout';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class HandsetStateService {
	isHandset$: Observable<BreakpointState>;

	constructor(private breakpointObserver: BreakpointObserver) {
		this.isHandset$ = this.breakpointObserver.observe([
			Breakpoints.XSmall,
			Breakpoints.Small,
		]);
	}
}
