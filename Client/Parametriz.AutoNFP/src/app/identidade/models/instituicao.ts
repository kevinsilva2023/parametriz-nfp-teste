export interface Instituicao {
  razaoSocial: string,
  cnpj: string,
  usuarioNome: Voluntario,
  email: string,
  senha: string,
  senhaConfirmacao: string
}

export interface Voluntario {
  email: string,
  senha: string
}
