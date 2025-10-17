import { Email } from "src/app/configuracoes/usuarios/models/usuario";

export class ObterUsuarioAtivo {
  id!: string;
  instituicaoId!: string;
  nome!: string;
  email!: Email;
  administrador!: boolean;
  desativado!: boolean
}