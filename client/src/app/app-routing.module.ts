import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {
	AuthenticationGuard,
	EmptyLayoutComponent,
	MainLayoutComponent,
} from '@expensely/core';

const routes: Routes = [
	{
		path: '',
		redirectTo: 'dashboard',
		pathMatch: 'full'
	},
	{
		path: '',
		component: MainLayoutComponent,
		children: [
			{
				path: 'dashboard',
				loadChildren: () => import('./modules/dashboard/dashboard.module').then((m) => m.DashboardModule),
				canLoad: [AuthenticationGuard]
			}
		]
	},
	{
		path: '',
		component: EmptyLayoutComponent,
		children: [
			{
				path: 'login',
				loadChildren: () => import('./modules/login/login.module').then((m) => m.LoginModule)
			}
		]
	},
	{
		path: '**',
		redirectTo: 'dashboard',
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
