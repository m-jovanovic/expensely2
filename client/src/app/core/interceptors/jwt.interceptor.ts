import { Injectable } from '@angular/core';
import {
	HttpInterceptor,
	HttpRequest,
	HttpHandler,
	HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationFacade } from '../store';

@Injectable({
	providedIn: 'root',
})
export class JwtInterceptor implements HttpInterceptor {
	constructor(private authenticationFacade: AuthenticationFacade) {}

	intercept(
		req: HttpRequest<any>,
		next: HttpHandler
	): Observable<HttpEvent<any>> {
		req = req.clone({
			setHeaders: {
				Authorization: `Bearer ${this.authenticationFacade.token}`,
			},
		});

		return next.handle(req);
	}
}
