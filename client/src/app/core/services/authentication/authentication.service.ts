import { Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from, of } from 'rxjs';
import { first, tap } from 'rxjs/operators';
import { ApiService } from '../api/api.service';
import { LoginRequest, TokenResponse } from '@expensely/core/contracts';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService extends ApiService {
	constructor(
		client: HttpClient,
		private router: Router,
		private zone: NgZone
	) {
		super(client);
	}

	login(request: LoginRequest): Observable<TokenResponse> {
		return this.post<TokenResponse>('authentication/login', request).pipe(
			first(),
			tap((response: TokenResponse) => {
				if (response.token) {
					this.zone.run(() => {
						this.router.navigate(['']);
					});
				}
			})
		);
	}

	logout(): Observable<boolean> {
		this.zone.run(() => {
			this.router.navigate(['/login']);
		});

		return of(true);
	}
}
