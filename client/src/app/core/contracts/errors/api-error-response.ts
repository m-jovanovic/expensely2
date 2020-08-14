import { ErrorItem as ApiError } from './api-error';
import { ErrorCode } from './error-codes.enum';

export class ApiErrorResponse {
	constructor(
		public success: boolean,
		public status: number,
		public errors: ApiError[]
	) {}

	hasError(errorCode: ErrorCode): boolean {
		return this.errors.some((e) => e.code === errorCode);
	}
}
