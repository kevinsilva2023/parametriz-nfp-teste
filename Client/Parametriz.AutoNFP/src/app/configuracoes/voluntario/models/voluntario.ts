import { Certificado } from "src/app/perfil/certificados/models/certificado";

export interface Voluntario {
  id: string,
  nome: string,
  cpf: string,
  email: string,
  contato: string,
  fotoUpload: string,
  administrador: boolean,
  desativado: boolean
  certificado: Certificado
}