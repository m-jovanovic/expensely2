import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { Login, Logout } from './authentication.actions';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationFacade {
	isLoggedIn$: Observable<boolean>;

	constructor(private store: Store) {
		this.isLoggedIn$ = this.store.select(
			(state) => !!state.authentication.token.length
		);
	}

	login(email: string, password: string): Observable<any> {
		return this.store.dispatch(new Login(email, password));
	}

	logout(): void {
		this.store.dispatch(new Logout());
	}
}
