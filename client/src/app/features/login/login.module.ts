import { NgModule } from '@angular/core';
import { SharedModule } from '@expensely/shared';
import { MaterialModule } from '@expensely/material';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './components/login/login.component';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
	declarations: [LoginComponent],
	imports: [
		SharedModule,
		MaterialModule,
		LoginRoutingModule,
		FlexLayoutModule,
		FormsModule,
	],
})
export class LoginModule {}
