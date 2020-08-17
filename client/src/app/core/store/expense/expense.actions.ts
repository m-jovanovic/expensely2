export class LoadExpenses {
	static readonly type = '[Expense] Load expenses';

	constructor(public limit: number, public cursor: string) {}
}

export class RemoveExpense {
	static readonly type = '[Expense] Remove expense';

	constructor(public id: string) {}
}
