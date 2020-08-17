import { Component, OnInit, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { Expense } from '@expensely/core/contracts';

@Component({
	selector: 'exp-expense-list',
	templateUrl: './expense-list.component.html',
	styleUrls: ['./expense-list.component.css'],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExpenseListComponent implements OnInit {
	@Input() expenses: Expense[];

	@Input() isLoading: boolean;

	@Output() removeExpense: EventEmitter<string> = new EventEmitter<string>();

	constructor() {}

	ngOnInit(): void {}

	remove(id: string): void {
		this.removeExpense.emit(id);
	}

	trackExpenses(index: number, expense: Expense): string {
		return expense.id;
	}
}
