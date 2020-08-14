import { Expense } from '../../contracts/expenses/expense';

export interface ExpensesStateModel {
	expenses: Expense[];
	cursor: string;
	more: boolean;
	isLoading: boolean;
}
