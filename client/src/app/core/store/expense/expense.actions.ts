export class LoadExpenses {
	static readonly type = '[Expenses] Load expenses';

	constructor(public limit: number, public cursor: string) {}
}
