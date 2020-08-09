import { Component, OnInit } from '@angular/core';
import { AuthenticationFacade } from '@expensely/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'exp-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string;
  password: string;
  isLoggedIn$: Observable<boolean>;

  constructor(private facade: AuthenticationFacade ) {
    this.isLoggedIn$ = this.facade.isLoggedIn$;
  }

  ngOnInit(): void {}

  signIn(): void {
    this.facade.signIn(this.email, this.password);
  }

  signOut(): void {
    this.facade.signOut();
  }

}
