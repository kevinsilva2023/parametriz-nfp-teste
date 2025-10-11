import { Email } from "./usuario";

export interface UsuarioCadastrado {
  nome: string,
  email: Email,
  administrador: number,
  desativado: number
}
