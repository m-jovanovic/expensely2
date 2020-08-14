import { Injectable, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from, of } from 'rxjs';
import { first, tap } from 'rxjs/operators';
import { ApiService } from '../api/api.service';
import { LoginRequest, TokenResponse } from '@expensely/core/contracts';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root'
})
export class AuthenticationService extends ApiService {

    constructor(client: HttpClient, private _router: Router, private _zone: NgZone) {
        super(client);
    }

    login(request: LoginRequest): Observable<TokenResponse> {
        return this.post<TokenResponse>('authentication/login', request)
            .pipe(
                first(),
                tap((response: TokenResponse) => {
                    if (response.token) {
                        this._zone.run(() => {
                            this._router.navigate(['']);
                        });
                    }
                })
            );
    }

    logout(): Observable<boolean> {
        this._zone.run(() => {
            this._router.navigate(['/login']);
        });

        return of(true);
    }

}