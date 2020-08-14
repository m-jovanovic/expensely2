import { Store, Select } from '@ngxs/store';
import { LoadExpenses } from './expense.actions';
import { Observable } from 'rxjs';
import { ExpenseState } from './expense.state';
import { Expense } from '../../contracts/expenses/expense';
import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class ExpenseFacade {
	@Select(ExpenseState.expenses)
	expenses$: Observable<Expense[]>;

	@Select(ExpenseState.isLoading)
	isLoading$: Observable<boolean>;

	constructor(private store: Store) {}

	getExpenses(limit: number): void {
		const more = this.store.selectSnapshot(ExpenseState.more);

		if (!more) {
			return;
		}

		const currentCursor = this.store.selectSnapshot<string>(
			ExpenseState.cursor
		);

		this.store.dispatch(new LoadExpenses(limit, currentCursor));
	}
}
