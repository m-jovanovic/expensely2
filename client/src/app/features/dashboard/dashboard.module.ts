import { NgModule } from '@angular/core';
import { SharedModule } from '@expensely/shared';
import { MaterialModule } from '@expensely/material';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { NgxsModule } from '@ngxs/store';
import { ExpenseState } from '@expensely/core';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
	declarations: [DashboardComponent, ExpenseListComponent],
	imports: [
		SharedModule,
		MaterialModule,
		DashboardRoutingModule,
		NgxsModule.forFeature([ExpenseState]),
		ScrollingModule,
		InfiniteScrollModule
	],
})
export class DashboardModule {}
