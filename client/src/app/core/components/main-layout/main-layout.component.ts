import { Component, OnInit } from '@angular/core';
import { AuthenticationFacade } from '@expensely/core/store';
import { Observable } from 'rxjs';

@Component({
  selector: 'exp-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit {
  isLoggedIn$: Observable<boolean>;

  constructor(private facade: AuthenticationFacade) {
    this.isLoggedIn$ = this.facade.isLoggedIn$;
  }

  ngOnInit(): void {
  }

  signOut(): void {
    this.facade.signOut();
  }

}
