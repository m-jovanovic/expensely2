import { ThemePalette } from '@angular/material/core/common-behaviors/color';

export interface ConfirmDialogData {
	title: string;
	message: string;
	dismissButtonText: string;
	dismissButtonColor: ThemePalette;
	confirmButtonText: string;
	confirmButtonColor: ThemePalette;
}
