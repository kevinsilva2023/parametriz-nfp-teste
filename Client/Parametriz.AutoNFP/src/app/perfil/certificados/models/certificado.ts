export interface Certificado {
    requerente: string,
    validoAPartirDe: Date,
    validoAte: Date,
    emissor: string,
    status: number,
    statusNome: string
}

