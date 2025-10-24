import { Voluntario } from "src/app/configuracoes/voluntarios/models/voluntario";

export class ErroTransmissaoLote {
  id!: string;
  instituicaoId!: string;
  voluntarioId?: string;
  data!: string;
  mensagem!: string;
  voluntario?: Voluntario;
} 