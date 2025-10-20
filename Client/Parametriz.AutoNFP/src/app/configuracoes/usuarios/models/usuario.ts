  export interface Usuario {
    id: string,
    nome: string,
    email: Email,
    fotoUpload?: string,
    administrador: number,
    desativado?: number;
  }

  export interface Email {
    conta: string
  }