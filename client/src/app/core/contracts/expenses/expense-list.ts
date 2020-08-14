import { Expense } from './expense';

export interface ExpenseList {
	items: Expense[];
	cursor: string;
}
