import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SignIn, SignOut } from './authentication.actions';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationFacade {
    isLoggedIn$: Observable<boolean>;

    constructor(private store: Store) {
        this.isLoggedIn$ = this.store.select(state => !!state.authentication.token.length);
    }

    signIn(email: string, password: string) {
        this.store.dispatch(new SignIn(email, password));
    }

    signOut() {
        this.store.dispatch(new SignOut());
    }

}