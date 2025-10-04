import { FormGroup } from "@angular/forms";
import { ValidationMessages } from "./validation-messages";
import { DisplayMessage } from "./display-message";

export class GenericFormValidator {

    constructor(private validationMessages: ValidationMessages) {}

    processarMensagens(formGroup: FormGroup): DisplayMessage {
        let displayMessages = new DisplayMessage();

        for (let controlKey in formGroup.controls) {
            if (formGroup.controls.hasOwnProperty(controlKey)) {
                let control = formGroup.controls[controlKey];

                if (control instanceof FormGroup) {
                    let childMessages = this.processarMensagens(control);
                    Object.assign(displayMessages, childMessages);
                } else {
                    if (this.validationMessages[controlKey]) {
                        displayMessages[controlKey] = '';
                        if ((control.dirty || control.touched) && control.errors) {
                            Object.keys(control.errors).map(messageKey => {
                                if (this.validationMessages[controlKey][messageKey]) {
                                    displayMessages[controlKey] += this.validationMessages[controlKey][messageKey] + '<br />';
                                }
                            });
                        }
                    }
                }
            }
        }
        return messages;
    }
}
