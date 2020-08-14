import { NgModule } from '@angular/core';
import { SharedModule } from '@expensely/shared';
import { MaterialModule } from '@expensely/material';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
	declarations: [DashboardComponent],
	imports: [SharedModule, MaterialModule, DashboardRoutingModule],
})
export class DashboardModule {}
