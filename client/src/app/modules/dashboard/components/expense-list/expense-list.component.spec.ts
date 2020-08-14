import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseListComponent } from './expense-list.component';

describe('ExpenseListComponent', () => {
	let component: ExpenseListComponent;
	let fixture: ComponentFixture<ExpenseListComponent>;

	beforeEach(async(() => {
		TestBed.configureTestingModule({
			declarations: [ExpenseListComponent],
		}).compileComponents();
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(ExpenseListComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
