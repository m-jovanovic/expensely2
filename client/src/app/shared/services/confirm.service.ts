import { Injectable } from '@angular/core';
import { ThemePalette } from '@angular/material/core/common-behaviors/color';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from '../contracts/confirm-dialog.data';

@Injectable({
	providedIn: 'root',
})
export class ConfirmService {
	constructor(private dialog: MatDialog) {}

	confirm(
		title: string = '',
		message: string = '',
		confirmButtonText: string = '',
		dismissButtonText: string = '',
		confirmButtonColor: ThemePalette = 'primary',
		dismissButtonColor: ThemePalette = 'primary'
	): Observable<boolean> {
		const data: ConfirmDialogData = {
			title,
			message,
			dismissButtonText,
			confirmButtonText,
			dismissButtonColor,
			confirmButtonColor,
		};

		const dialogRef: MatDialogRef<
			ConfirmDialogComponent,
			boolean
		> = this.dialog.open(ConfirmDialogComponent, {
			maxWidth: '350px',
			width: '350px',
			data,
		});

		return dialogRef.afterClosed();
	}
}
