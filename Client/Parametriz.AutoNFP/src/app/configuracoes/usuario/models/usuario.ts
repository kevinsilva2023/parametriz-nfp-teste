  export interface Usuario {
    id: string,
    nome: string,
    email: Email,
    administrador: number,
    desativado?: number;
  }

  export interface Email {
    conta: string
  }