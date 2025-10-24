import { Voluntario } from "src/app/configuracoes/voluntarios/models/voluntario";

export class CupomFiscalPaginacao {
  cuponsFiscais!: CupomFiscal[];
  pagina!: number;
  registrosPorPagina!: number;
  processando!: number;
  sucesso!: number;
  erro!: number;
  total!: number;
}

export class CupomFiscal {
  id!: string;
  instituicaoId!: string;
  chave!: string;
  numero!: number;
  cnpj!: string;
  competencia!: string;
  cadastradoPorId!: string;
  cadastradoEm!: string;
  status!: number;
  statusNome!: string;
  enviadoEm!: string;
  mensagemErro!: string;
  cadastradoPor!: Voluntario;
}

