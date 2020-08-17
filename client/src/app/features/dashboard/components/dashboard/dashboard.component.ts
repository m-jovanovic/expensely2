import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ExpenseFacade, Expense } from '@expensely/core';

@Component({
	selector: 'exp-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
	private readonly limit = 15;
	expenses$: Observable<Expense[]>;
	isLoading$: Observable<boolean>;

	constructor(private expenseFacade: ExpenseFacade) {
		this.expenses$ = this.expenseFacade.expenses$;
		this.isLoading$ = this.expenseFacade.isLoading$;
	}

	ngOnInit(): void {
		this.expenseFacade.getExpenses(this.limit);
	}

	removeExpense(id: string): void {
		// TODO: Add a confirmation dialog.
		this.expenseFacade.removeExpense(id);
	}

	onScroll(): void {
		this.expenseFacade.getExpenses(this.limit);
	}
}
