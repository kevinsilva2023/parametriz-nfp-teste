import { CustomBadResponseErrors } from "./custom-bad-response-errors";

export class CustomBadResponse {
    title: string;
    status: number;
    errors: CustomBadResponseErrors;

    constructor() {
        this.title = '';
        this.status = 0;
        this.errors = new CustomBadResponseErrors();
    }
}

