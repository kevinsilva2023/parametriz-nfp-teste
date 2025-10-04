import { CustomBadResponse } from "./custom-bad-response";

export class CustomResponse {
    error: CustomBadResponse;

    constructor() {
        this.error = new CustomBadResponse();
    }
}
