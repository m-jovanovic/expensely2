import { ApiService } from '../api/api.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ExpenseList } from '../../contracts/expenses/expense-list';

@Injectable({
	providedIn: 'root',
})
export class ExpenseService extends ApiService {
	constructor(client: HttpClient) {
		super(client);
	}

	getExpenses(limit: number, cursor: string): Observable<ExpenseList> {
		return this.get<ExpenseList>(`expenses?limit=${limit}&cursor=${cursor}`);
	}

	removeExpense(id: string): Observable<any> {
		return this.delete(`expenses/${id}`);
	}
}
