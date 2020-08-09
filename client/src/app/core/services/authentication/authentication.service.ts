import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { first, tap } from 'rxjs/operators';
import { ApiService } from '../api/api.service';
import { LoginRequest, TokenResponse } from '@expensely/core/contracts';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root'
})
export class AuthenticationService extends ApiService {

    constructor(client: HttpClient, private router: Router) {
        super(client);
    }

    signIn(email: string, password: string): Observable<TokenResponse> {
        const request = new LoginRequest(email, password);

        const response = this.post<TokenResponse>('authentication/login', request)
            .pipe(
                first(),
                tap((response: TokenResponse) => {
                    if (response.token) {
                        this.router.navigate(['/']);
                    }
                })
            );

        return response;
    }

    signOut(): Observable<boolean> {
        return from(this.router.navigate(['/login']));
    }

}