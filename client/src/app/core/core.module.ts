import { NgModule, Optional, SkipSelf } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmptyLayoutComponent } from './components/empty-layout/empty-layout.component';
import { HttpClientModule } from '@angular/common/http';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';
import { SharedModule } from '@expensely/shared';
import { MaterialModule } from '@expensely/material';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { SideNavMenuComponent } from './components/side-nav-menu/side-nav-menu.component';

@NgModule({
	declarations: [EmptyLayoutComponent, MainLayoutComponent, SideNavComponent, NavBarComponent, SideNavMenuComponent],
	imports: [
		HttpClientModule,
		RouterModule,
		FormsModule,
		ReactiveFormsModule,
		SharedModule,
		MaterialModule
	],
	providers: [],
	exports: [EmptyLayoutComponent]
})
export class CoreModule {
	constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
		if (parentModule) {
			throw new Error('The Core module has already been loaded.');
		}
	}
}
