export class DefinirSenha {
    email: string;
    senha: string;
    senhaConfirmacao: string;
    code: string;

    constructor(email: string, senha: string, senhaConfirmacao: string, code: string) {
        this.email = email;
        this.senha = senha;
        this.senhaConfirmacao = senhaConfirmacao;
        this.code = code;
    }
}
