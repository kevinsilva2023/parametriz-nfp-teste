export class EnviarConfirmarEmail {
    usuarioId: string;
    definirSenha: boolean;

    constructor(usuarioId: string, definirSenha: boolean) {
        this.usuarioId = usuarioId;
        this.definirSenha = definirSenha;
    }
}
