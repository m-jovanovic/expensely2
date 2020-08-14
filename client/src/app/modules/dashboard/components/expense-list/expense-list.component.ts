import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Expense } from '@expensely/core/contracts';

@Component({
	selector: 'exp-expense-list',
	templateUrl: './expense-list.component.html',
	styleUrls: ['./expense-list.component.css'],
})
export class ExpenseListComponent implements OnInit {
	@Input() expenses: Expense[];

	@Input() isLoading: boolean;

	constructor() {}

	ngOnInit(): void {}

	trackExpenses(index: number, expense: Expense): string {
		return expense.id;
	}
}
