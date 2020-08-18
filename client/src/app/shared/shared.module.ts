import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { MaterialModule } from '@expensely/material';

@NgModule({
	declarations: [ConfirmDialogComponent],
	imports: [CommonModule, MaterialModule],
	exports: [CommonModule],
})
export class SharedModule {}
