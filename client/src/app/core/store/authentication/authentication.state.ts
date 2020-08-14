import { Injectable } from '@angular/core';
import { State, StateContext, Action } from '@ngxs/store';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthenticationService } from '../../services';
import { AuthenticationStateModel } from './authentication-state.model';
import { Login, Logout } from './authentication.actions';
import { TokenResponse, LoginRequest } from '../../contracts';

@State<AuthenticationStateModel>({
    name: 'authentication',
    defaults: {
        token: ''
    }
})
@Injectable()
export class AuthenticationState {

    constructor(private authenticationService: AuthenticationService) {}

    @Action(Login)
    login(context: StateContext<AuthenticationStateModel>, action: Login): Observable<TokenResponse> {
        return this.authenticationService.login(new LoginRequest(action.email, action.password)).pipe(
            tap((response: TokenResponse) => {
                context.patchState({
                    token: response.token
                });
            })
        );
    }

    @Action(Logout)
    logout(context: StateContext<AuthenticationStateModel>): Observable<boolean> {
        return this.authenticationService.logout().pipe(
            tap(() => {
                context.patchState({
                    token: ''
                });
            })
        );
    }

}