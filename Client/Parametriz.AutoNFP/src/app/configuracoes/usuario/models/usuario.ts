export interface Usuario {
  nome: string,
  email: Email,
  administrador: number,
  desativado?: number;
}

export interface Email {
  conta: string
}