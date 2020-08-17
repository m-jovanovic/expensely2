import { Injectable } from '@angular/core';
import { State, StateContext, Action, Selector } from '@ngxs/store';
import { ExpensesStateModel } from './expense-state.model';
import { Observable, throwError, of } from 'rxjs';
import { LoadExpenses, RemoveExpense } from './expense.actions';
import { ExpenseList } from '../../contracts/expenses/expense-list';
import { ExpenseService } from '../../services/expense/expense.service';
import { tap, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Expense } from '../../contracts/expenses/expense';

@State<ExpensesStateModel>({
	name: 'expense',
	defaults: {
		expenses: [],
		cursor: '',
		isLoading: false,
		more: true,
	},
})
@Injectable()
export class ExpenseState {
	@Selector()
	static expenses(state: ExpensesStateModel): Expense[] {
		return state.expenses;
	}

	@Selector()
	static cursor(state: ExpensesStateModel): string {
		return state.cursor;
	}

	@Selector()
	static more(state: ExpensesStateModel): boolean {
		return state.more;
	}

	@Selector()
	static isLoading(state: ExpensesStateModel): boolean {
		return state.isLoading;
	}

	constructor(private expenseService: ExpenseService) {}

	@Action(LoadExpenses)
	loadExpenses(
		context: StateContext<ExpensesStateModel>,
		action: LoadExpenses
	): Observable<ExpenseList> {
		context.patchState({
			isLoading: true,
		});

		return this.expenseService.getExpenses(action.limit, action.cursor).pipe(
			tap((response) => {
				const currentState = context.getState();

				context.patchState({
					expenses: currentState.expenses.concat(response.items),
					cursor: response.cursor,
					more: !!response.cursor,
					isLoading: false,
				});
			}),
			catchError((error: HttpErrorResponse) => {
				context.patchState({
					isLoading: false,
				});

				return throwError(error);
			})
		);
	}

	@Action(RemoveExpense)
	removeExpense(
		context: StateContext<ExpensesStateModel>,
		action: RemoveExpense
	): Observable<any> {
		const initialExpenses = context.getState().expenses;

		const filteredExpenses = initialExpenses.filter(
			(expense) => expense.id !== action.id
		);

		context.patchState({
			expenses: filteredExpenses,
		});

		return this.expenseService.removeExpense(action.id).pipe(
			catchError((error: HttpErrorResponse) => {
				context.patchState({
					expenses: initialExpenses,
				});

				return throwError(error);
			})
		);
	}
}
