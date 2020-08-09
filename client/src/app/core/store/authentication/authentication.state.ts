import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { AuthenticationService } from '../../services';
import { AuthenticationStateModel } from './authentication-state.model';
import { SignIn, SignOut } from './authentication.actions';
import { tap, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { TokenResponse } from '../../contracts';
import { Router } from '@angular/router';

@State<AuthenticationStateModel>({
    name: 'authentication',
    defaults: {
        token: '',
        permissions: []
    }
})
@Injectable()
export class AuthenticationState {

    constructor(private authenticationService: AuthenticationService, private router: Router) {}

    @Action(SignIn)
    signIn(context: StateContext<AuthenticationStateModel>, action: SignIn) {
        return this.authenticationService.signIn(action.email, action.password)
            .pipe(
                tap((response: TokenResponse) => {
                    context.patchState({
                        token: response.token,
                        permissions: ['AccessEverything']
                    });
                }),
                catchError((err: HttpErrorResponse) => {
                    console.log(err);

                    return throwError(err.message);
                })
            );
    }

    @Action(SignOut)
    signOut(context: StateContext<AuthenticationStateModel>): Observable<boolean> {
        return this.authenticationService.signOut().pipe(
            tap(() => {
                context.patchState({
                    token: '',
                    permissions: []
                });
            })
        );
    }

}