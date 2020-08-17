import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

import { AuthenticationFacade } from '@expensely/core';
import { ApiErrorResponse, ErrorCode } from '@expensely/core/contracts';

@Component({
	selector: 'exp-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css'],
})
export class LoginComponent {
	email: string;
	password: string;
	hide = true;
	isLoggedIn$: Observable<boolean>;

	constructor(private facade: AuthenticationFacade) {
		this.isLoggedIn$ = this.facade.isLoggedIn$;
	}

	login(): void {
		this.facade
			.login(this.email, this.password)
			.pipe(
				catchError((err: HttpErrorResponse) => {
					this.handleError(err.error);

					return of('');
				})
			)
			.subscribe();
	}

	private handleError(apiError: ApiErrorResponse): void {
		if (apiError.hasError(ErrorCode.UserNotFound)) {
			// Invalid email

			return;
		}

		if (apiError.hasError(ErrorCode.InvalidPassword)) {
			// Invalid password

			return;
		}

		if (
			apiError.hasError(ErrorCode.EmailNullOrEmpty) ||
			apiError.hasError(ErrorCode.PasswordNullOrEmpty)
		) {
			// Invalid inputs

			return;
		}
	}
}
