import { Endereco } from "src/app/shared/models/endereco";

export class CadastrarInstituicao {
  cnpj!: string;
  razaoSocial!: string;
  entidadeNomeNFP!: string;
  endereco!: Endereco;
  voluntarioNome!: string;
  cpf!: string;
  email!: string;
  contato!: string;
}
