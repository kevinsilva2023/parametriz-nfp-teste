export interface ConsultarVoluntario {
  nome: string,
  cnpjCpf: CpfCnpj,
  requerente: string,
  validoAPartirDe: string,
  validoAte: string,
  emissor: string,
  status: string
}

export interface CpfCnpj {
  numeroInscricao: string
}