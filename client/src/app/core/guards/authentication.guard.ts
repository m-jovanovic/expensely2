import { Injectable } from '@angular/core';
import {
	CanActivate,
	CanLoad,
	Route,
	UrlSegment,
	ActivatedRouteSnapshot,
	RouterStateSnapshot,
	Router,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationFacade } from '../store/authentication/authentication.facade';
import { take, tap, map } from 'rxjs/operators';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationGuard implements CanActivate, CanLoad {
	constructor(private router: Router, private facade: AuthenticationFacade) {}

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean> {
		return this.checkAuthentication();
	}

	canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
		return this.checkAuthentication();
	}

	private checkAuthentication(): Observable<boolean> {
		return this.facade.isLoggedIn$.pipe(
			take(1),
			tap((loggedIn: boolean) => {
				if (!loggedIn) {
					this.router.navigate(['/login']);
				}
			})
		);
	}
}
