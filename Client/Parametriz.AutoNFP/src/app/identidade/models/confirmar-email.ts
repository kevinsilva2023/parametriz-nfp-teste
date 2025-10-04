export class ConfirmarEmail {
    email: string;
    code: string;
    definirSenha: boolean;

    constructor(email: string, code: string, definirSenha: boolean) {
        this.email = email;
        this.code = code;
        this.definirSenha = definirSenha;
    }
}
